import asyncio
from fastapi import FastAPI
from pydantic import BaseModel
from aiogram import Bot, Dispatcher, types
from aiogram.types import InlineKeyboardMarkup, InlineKeyboardButton
import uvicorn

TOKEN = "YOUR_BOT_TOKEN"
bot = Bot(token=TOKEN)
dp = Dispatcher(bot)
app = FastAPI()

class PollRequest(BaseModel):
    chat_id: int
    doctor_id: int


@dp.message_handler(commands=['start'])
async def start_handler(message: types.Message):
    args = message.text.split()
    if len(args) > 1:
        token = args[1]
        
        # TODO: ... логика работы с MongoDB ...
        
        await message.reply("Аккаунт успешно привязан! Выберите статус на сегодня:", 
                            reply_markup=get_keyboard("doctor_id_from_token"))
    else:
        await message.reply("Добро пожаловать! Если вы доктор, перейдите по ссылке из почты.")

def get_keyboard(doctor_id):
    keyboard = InlineKeyboardMarkup(row_width=1)
    keyboard.add(
        InlineKeyboardButton("✅ At work", callback_data=f"status_work_{doctor_id}"),
        InlineKeyboardButton("🤒 Sick Day", callback_data=f"status_sick_{doctor_id}"),
    InlineKeyboardButton("🚐 On vacation", callback_data=f"status_vacation_{doctor_id}")

    )
    return keyboard

@app.post("/send-daily-poll")
async def send_daily_poll(request: PollRequest):    
    try:
        await bot.send_message(
            chat_id=request.chat_id,
            text="Доброе утро! Какой у вас сегодня статус?",
            reply_markup=get_keyboard(request.doctor_id)
        )
        return {"status": "ok"}
    except Exception as e:
        return {"status": "error", "message": str(e)}

async def main():
    config = uvicorn.Config(app=app, host="0.0.0.0", port=8000)
    server = uvicorn.Server(config)
    asyncio.create_task(server.serve())

    await dp.start_polling()

if name == 'main':
    asyncio.run(main())