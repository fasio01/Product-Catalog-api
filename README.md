# 🛒 Product Catalog API — .NET 8 Web API with File Upload

This is a simple yet practical RESTful API built using **ASP.NET Core 8**, designed to manage a catalog of products. 
It supports full **CRUD operations** and **image file uploads**, making it ideal for internship-level portfolios and learning backend API development.

---

## 🔧 Technologies Used

- ASP.NET Core 8 Web API  
- Entity Framework Core  
- SQL Server  
- C#  
- IFormFile (File Upload)  
- Dependency Injection  
- LINQ  

---

## 📦 Features

- Add a new product with image upload  
- Retrieve all products with image URLs  
- Update product details with optional image replacement  
- Delete a product and its image from server  
- File validation: allowed extensions and max size  
- Images saved to `wwwroot/uploadedfiles/`  

---

## 📋 Product Model

The `product` model contains the following fields:

| Field        | Type        | Description                             |
|--------------|-------------|-----------------------------------------|
| Id           | int         | Auto-generated primary key              |
| ProductName  | string      | Name of the product                     |
| Price        | string      | Price of the product                    |
| Description  | string      | Product description                     |
| File         | IFormFile   | Used for uploading an image or PDF      |
| ImagePath    | string      | File name stored on the server          |

> Note: `File` is `[NotMapped]` and used only during upload.

---

## 📂 API Endpoints

### ➕ Add Product  
- **URL:** `POST /api/Home/add`  
- **Consumes:** `multipart/form-data`  
- **Fields:**  
  - `ProductName` (string)  
  - `Price` (string)  
  - `Description` (string)  
  - `File` (image or PDF: `.jpg`, `.jpeg`, `.png`, `.pdf`)  

**Sample Response:**
```json
{
  "Id": 1,
  "Productname": "Sample Product",
  "Price": "1000",
  "Desc": "This is a test product",
  "File": "abc.jpg",
  "Image": "http://localhost:5000/uploadedfiles/abc.jpg",
  "Message": "data added successfully"
}
