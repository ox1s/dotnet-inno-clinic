# dotnet-inno-clinic
Innowise Clinic ASP.NET Core Web API (Pre-Trainee Innowise assignment) 


## Схема сущностей представленных в базах данных, распределенных по соответствующим сервисам
<details>
<summary><b>📸 Показать схему бд</b></summary>
  <br>
  <div align="center">
    <img src="_assets/db_shemas.png" alt="Db shemas" width="1000">
  </div>
</details>

<br>
Соответсвенно распределение по Bounded Context будет следующим
<p>

<details>
<summary><b>📸 Показать BC</b></summary>
  <br>
  <div align="center">
    <img src="_assets/bounded_context.png" alt="Bounded context" width="1000">
  </div>
</details>



### 👤 Identity Service 
Микросервис, отвечающий за управление регистрацией пользователей.
<details>
  <summary><b>📸 Показать схему домена Account</b></summary>
  <br>
  <div align="center">
    <img src="images/UserManagementDomain.png" alt="User Management Domain" width="800">
  </div>
</details>

### 📦 Product Management Service
Микросервис каталога товаров. Управляет продуктами, категориями, ценами и списками желаемого. Хранит денормализованную реплику данных о продавце для оптимизации чтения.
<details>
  <summary><b>📸 Показать схему домена Product</b></summary>
  <br>
  <div align="center">
    <img src="images/ProductManagementDomain.png" alt="Product Management Domain" width="800">
  </div>
</details>

---
