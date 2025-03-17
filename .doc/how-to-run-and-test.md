[Back to README](../README.md)

# How to Run and Test the Project

## **READ CAREFULLY**

---

## **Instructions**

To run the project, make sure you have the following tools installed and working:

- **Git**: To clone or download the repository.
- **Docker and Docker Compose**: Required to run the project containers.
- **Visual Studio**: To run and debug the application locally.
- **Database management tool**: Such as **DBeaver** or **PGAdmin** to access PostgreSQL.
- **RedisInsight**: To visualize the Redis database.
- **Postman**: To test API requests.

---

## **Running the Project via Docker Compose**

### **1. Clone the Repository**

Download or clone the project from the GitHub repository:

```bash
  git clone <repository-url>
  cd template/backend
```

### **2. Start the Containers**

In the `backend` folder, open a terminal (CMD, PowerShell, or VS Code Terminal) and run:

```bash
  docker compose up -d
```

This will start the following containers:

- **webapi**: The application container.
- **postgres**: PostgreSQL database.
- **mongodb**: MongoDB database (if applicable).
- **redis**: For caching.
- **rabbitmq**: For message brokering (**Management UI available at port 15672**).

### **3. Checking Services**

- **Swagger UI**: Access the API documentation at `http://localhost:8080/swagger/index.html`
- **RabbitMQ**: Access the management interface at `http://localhost:15672`
  - **Username**: `developer`
  - **Password**: `ev@luAt10n`
- **PostgreSQL**:
  - **Host**: `localhost` or `postgres` (if connected via Docker)
  - **Port**: `5432`
  - **Database**: `developer_evaluation`
  - **Username**: `developer`
  - **Password**: `ev@luAt10n`
- **Redis**:
  - **Host**: `localhost` or `redis`
  - **Port**: `6379`
  - **Username**: (leave empty)
  - **Password**: `ev@luAt10n`
  - Use **RedisInsight** to visualize the data.

---

## **Running the Project via Visual Studio**

1. **Open the Solution**
   - Open Visual Studio and load the project solution.
2. **Set the Environment**
   - Ensure the environment is set to `Development` in `Properties/launchSettings.json`.
3. **Run the Application**
   - Press `F5` or `Ctrl+F5` to start.
   - If `Database:Migrate` and `Database:Seed` are set to `true`, migrations will be applied, and the database will be seeded with initial data.
4. **Access Swagger UI**
   - Visit `https://localhost:44312/swagger/index.html` to test API endpoints.

---

## **Generating and Applying Migrations**

- **Via Application Startup:**

  - Start the application via Visual Studio. If migrations are enabled, they will run automatically on startup. You can check the status at `https://localhost:44312/swagger/index.html`.

- **Via Visual Studio (Package Manager Console):**

```powershell
  Update-Database
```

---

## **Accessing RabbitMQ**

- **RabbitMQ Management Console:**
  - URL: `http://localhost:15672`
  - Username: `developer`
  - Password: `ev@luAt10n`

- **Exchange and Queue:**
  - **Exchange**: `ambev.sales.exchange`
  - **Queue**: `ambev.sales.queue`

  Use the **Queues** section in RabbitMQ to monitor incoming messages and ensure the integration is working properly.

---

## **Visualizing the Database**

- **PostgreSQL** (via **DBeaver** or **PGAdmin**):
  - **Host**: `localhost` or `postgres`
  - **Port**: `5432`
  - **Database**: `developer_evaluation`
  - **Username**: `developer`
  - **Password**: `ev@luAt10n`

- **Redis** (via **RedisInsight**):
  - **Host**: `localhost` or `redis`
  - **Port**: `6379`
  - **Password**: `ev@luAt10n`

---

## **Testing API with Postman**

A Postman collection containing all API endpoints is available in the `./doc/postman` folder.

### **How to Import:**

1. Open Postman.
2. Click **Import** and select the collection file from the `.doc/postman` folder.
3. The collection includes example requests for all endpoints (Users, Products, Carts, Sales, etc.), with sample JSON bodies and usage instructions.

---

## **Event-Driven Architecture**

The system automatically sends events to RabbitMQ whenever there are changes in the sales status. The following events are published:

- **SaleCreated**
- **SaleModified**
- **SaleCancelled**
- **ItemCancelled**

These events enable a more dynamic and scalable system, allowing real-time integration with other services.

---

## **Notes**

- **Database & Cache Monitoring**:
  - Use **PGAdmin** or **DBeaver** to manage PostgreSQL.
  - Use **RedisInsight** to monitor Redis.

---

## **Happy Coding!** 🚀
