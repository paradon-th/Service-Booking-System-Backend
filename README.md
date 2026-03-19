# SBS (Service Booking System) - Backend

ระบบหลังบ้าน (Backend) ของโปรเจกต์ SBS พัฒนาด้วย **ASP.NET Core (API)** โดยใช้สถาปัตยกรรมแบบ **Clean Architecture** เพื่อให้โค้ดมีความเป็นระเบียบ แบ่งแยกความรับผิดชอบ และง่ายต่อการทดสอบและการดูแลระยะยาว

## 🏗 สถาปัตยกรรม (Architecture)
โปรเจกต์นี้แบ่งออกเป็น 4 โปรเจกต์ย่อยตามหลัก Clean Architecture:

1.  **SBS.API (Presentation Layer)**
    *   ทำหน้าที่เป็นทางเข้าหลักของระบบผ่าน RESTful API
    *   จัดการเรื่อง Authentication (JWT Bearer) และ Authorization
    *   คอนฟิกูเรชันต่างๆ เช่น CORS, Swagger และ Dependency Injection
    *   **Controllers:** จุดรับ Request และส่งต่อให้ Application Layer

2.  **SBS.Application (Application Layer)**
    *   บรรจุ Business Logic และเงื่อนไขการทำงานของระบบ (Services)
    *   กำหนด Interfaces เพื่อใช้ในการสื่อสารข้ามชั้น
    *   ใช้ DTOs (Data Transfer Objects) ในการรับส่งข้อมูลเพื่อความปลอดภัยและประสิทธิภาพ

3.  **SBS.Domain (Domain Layer)**
    *   เป็นส่วนที่เป็นหัวใจสำคัญ (Core) ของระบบ ไม่ขึ้นตรงกับ Layer อื่น
    *   **Entities:** โมเดลข้อมูลหลักที่สะท้อนถึง Business Domain
    *   **Enums:** ประเภทข้อมูลต่างๆ ที่ใช้ในระบบ
    *   **Interfaces:** กำหนดมาตรฐานสำหรับ Repository

4.  **SBS.Infrastructure (Infrastructure Layer)**
    *   จัดการการติดต่อกับข้อมูลภายนอก (Data Access)
    *   **PostgreSQL:** ใช้ Entity Framework Core เป็น ORM ในการจัดการฐานข้อมูล
    *   **Repositories:** การเขียนโปรแกรมจัดการข้อมูลจริง (Implementation of Domain Interfaces)
    *   **Migrations:** ระบบจัดการเวอร์ชันของฐานข้อมูล

## 🛠 เทคโนโลยีที่ใช้ (Tech Stack)
*   **Backend:** ASP.NET Core (C#)
*   **Database:** PostgreSQL
*   **ORM:** Entity Framework Core
*   **Authentication:** JWT (JSON Web Token)
*   **Documentation:** Swagger / OpenAPI
*   **Design Patterns:** Repository Pattern, Unit of Work, Dependency Injection

## 🚀 วิธีการเริ่มต้นใช้งาน (Getting Started)

### 📋 สิ่งที่ต้องมี (Prerequisites)
*   [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop) (สำหรับรันฐานข้อมูลผ่าน Docker Compose)

### 🏎️ ขั้นตอนการรัน
1.  **รันฐานข้อมูลด้วย Docker:**
    ```bash
    docker-compose up -d
    ```
2.  **ตั้งค่าฐานข้อมูล (Migrations):**
    หากมีการแจ้งเตือนว่าฐานข้อมูลยังไม่ถูกสร้าง ให้รันคำสั่ง:
    ```bash
    dotnet ef database update --project SBS.Infrastructure --startup-project SBS.API
    ```
3.  **รันแอปพลิเคชัน:**
    ```bash
    cd SBS.API
    dotnet run
    ```
    หรือใช้งานผ่าน Visual Studio / VS Code Debugger

## 🔗 API Documentation
เมื่อรันแอปพลิเคชันแล้ว สามารถเข้าดูรายละเอียด API ได้ที่:
`http://localhost:<port>/swagger`
