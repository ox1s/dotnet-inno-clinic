import os
import uuid
import logging
import sqlite3
import asyncio
import sys
import random
import httpx
from dotenv import load_dotenv
from telegram import Update, InlineKeyboardButton, InlineKeyboardMarkup
from telegram.ext import Application, CommandHandler, CallbackQueryHandler, ContextTypes

# WARNINING!!!!!!!!!!!!!!!!!⚠️⚠️⚠️⚠️⚠️⚠️⚠️⚠️⚠️⚠️⚠️⚠️⚠️
# ПИСАЛА НЕ Я ЗАСЛУГА НЕ МОЯ, НО ЧЕМ Я ОТЛИЧАЮСЬ ОТ ПАЦАНОВ ЧТО ДЕЛАЛИ НА МИТАП ТЕЛЕГРАМ БОТА С ПАРКОВКОЙ И ИИ?))))))💅💅💅💅💅💅💅

# Загрузка переменных окружения
load_dotenv()
TOKEN = os.getenv("TELEGRAM_BOT_TOKEN")
BACKEND_URL = os.getenv("BACKEND_API_URL")
API_KEY = os.getenv("API_KEY")

# Настройка логирования
logging.basicConfig(format="[%(levelname)s] - %(asctime)s - %(name)s %(message)s", level=logging.INFO)

logging.getLogger("httpx").setLevel(logging.WARNING)
logging.getLogger("telegram").setLevel(logging.WARNING)

logger = logging.getLogger(__name__)

# --- БАЗА ДАННЫХ (Локальная для бота) ---
# Боту нужно помнить, какой TelegramId какому AccountId принадлежит.
def init_db():
    with sqlite3.connect("bot_database.db") as conn:
        conn.execute("""
            CREATE TABLE IF NOT EXISTS users (
                telegram_id INTEGER PRIMARY KEY,
                account_id TEXT NOT NULL
            )
        """)

def save_user(telegram_id: int, account_id: str):
    with sqlite3.connect("bot_database.db") as conn:
        conn.execute("REPLACE INTO users (telegram_id, account_id) VALUES (?, ?)", (telegram_id, account_id))

def get_account_id(telegram_id: int) -> str:
    with sqlite3.connect("bot_database.db") as conn:
        cursor = conn.execute("SELECT account_id FROM users WHERE telegram_id = ?", (telegram_id,))
        row = cursor.fetchone()
        return row[0] if row else None

# --- КЛАВИАТУРА СО СТАТУСАМИ ---
def get_status_keyboard():
    keyboard = [
        [InlineKeyboardButton("🏢 At work", callback_data="At work"), InlineKeyboardButton("🌴 On vacation", callback_data="On vacation")],
        [InlineKeyboardButton("🤒 Sick Day", callback_data="Sick Day"), InlineKeyboardButton("🏥 Sick Leave", callback_data="Sick Leave")],
        [InlineKeyboardButton("🏠 Self-isolation", callback_data="Self-isolation"), InlineKeyboardButton("🚫 Leave without pay", callback_data="Leave without pay")]
    ]
    return InlineKeyboardMarkup(keyboard)

# --- ОБРАБОТЧИКИ КОМАНД ---

async def start(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """
    Обрабатывает переход по ссылке вида: https://t.me/bot?start=3fa85f6457174562b3fc2c963f66afa6
    """
    telegram_id = update.effective_user.id
    args = context.args # Аргументы после /start

    if args:
        raw_token = args[0]
        try:
            # Превращаем токен без дефисов обратно в стандартный Guid для C#
            account_id = str(uuid.UUID(raw_token))
            
            # 1. Сохраняем локально в боте
            save_user(telegram_id, account_id)
            
            # 2. Отправляем на C# бэкенд, чтобы он записал TelegramId в MongoDB
            async with httpx.AsyncClient() as client:
                headers = {"X-Api-Key": API_KEY}
                payload = {"accountId": account_id, "telegramId": str(telegram_id)}
                
                response = await client.post(f"{BACKEND_URL}/bot/accounts/link-telegram", json=payload, headers=headers)
                response.raise_for_status()

            await update.message.reply_text(
                "✅ Ваш аккаунт успешно привязан к Telegram!\nТеперь вы можете отмечать свой статус.",
                reply_markup=get_status_keyboard()
            )
            logger.info(f"Linked TelegramId {telegram_id} to AccountId {account_id}")

        except ValueError:
            await update.message.reply_text("❌ Неверный формат токена.")
        except httpx.HTTPError as e:
            logger.error(f"Backend error during linking: {e}")
            await update.message.reply_text("⚠️ Ошибка связи с сервером клиники. Попробуйте позже.")
    else:
        # Если пользователь просто нажал /start без токена
        account_id = get_account_id(telegram_id)
        if account_id:
            await update.message.reply_text("Выберите ваш текущий статус:", reply_markup=get_status_keyboard())
        else:
            await update.message.reply_text("⚠️ Пожалуйста, перейдите по ссылке из письма, чтобы привязать аккаунт.")

async def status_command(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Позволяет вызвать меню статусов командой /status"""
    telegram_id = update.effective_user.id
    if get_account_id(telegram_id):
        await update.message.reply_text("Выберите ваш текущий статус:", reply_markup=get_status_keyboard())
    else:
        await update.message.reply_text("⚠️ Сначала привяжите аккаунт по ссылке из письма.")

async def button_click(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Обрабатывает нажатие на кнопку статуса"""
    query = update.callback_query
    await query.answer() # Убираем "часики" загрузки на кнопке

    telegram_id = update.effective_user.id
    selected_status = query.data
    account_id = get_account_id(telegram_id)

    if not account_id:
        await query.edit_message_text("⚠️ Ошибка: Аккаунт не привязан.")
        return

    try:
        # Отправляем PUT запрос в C# API (ProfilesApi)
        async with httpx.AsyncClient() as client:
            headers = {"X-Api-Key": API_KEY}
            payload = {
                "accountId": account_id,
                "status": selected_status
            }
            
            response = await client.put(f"{BACKEND_URL}/bot/doctors/status", json=payload, headers=headers)
            response.raise_for_status() # Выкинет ошибку, если статус не 200 OK

        # Обновляем сообщение в телеграме
        await query.edit_message_text(f"✅ Ваш статус успешно изменен на: **{selected_status}**", parse_mode="Markdown")
        logger.info(f"Status updated for {account_id} to {selected_status}")

    except httpx.HTTPStatusError as e:
        logger.error(f"HTTP Error {e.response.status_code}: {e.response.text}")
        await query.edit_message_text("❌ Ошибка при обновлении статуса. Возможно, вы не врач.")
    except Exception as e:
        logger.error(f"Connection error: {e}")
        await query.edit_message_text("⚠️ Нет связи с сервером. Попробуйте позже.")

# --- ЗАПУСК БОТА ---
if __name__ == "__main__":
    init_db() # Создаем табличку при старте
    
    if sys.platform == 'win32':
        asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())
        
    try:
        loop = asyncio.get_event_loop()
    except RuntimeError:
        loop = asyncio.new_event_loop()
        asyncio.set_event_loop(loop)
    # ==========================================

    app = Application.builder().token(TOKEN).build()

    # Регистрируем обработчики
    app.add_handler(CommandHandler("start", start))
    app.add_handler(CommandHandler("status", status_command))
    app.add_handler(CallbackQueryHandler(button_click))

    logger.info("Bot is running...")
    app.run_polling()