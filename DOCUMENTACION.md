# Documentación - MiniSocialMediaAPI

## 📋 Descripción General

Esta es una API REST para una mini red social desarrollada en **ASP.NET Core**. Permite a los usuarios crear publicaciones, interactuar con otras personas mediante likes y comentarios, chatear en privado y formar grupos.

---

## 🏗️ Estructura de la Aplicación

### **Program.cs**

Archivo principal donde se configuran todos los servicios y middlewares de la aplicación:

- **Swagger**: Documentación interactiva de la API
- **CORS**: Configuración de acceso desde otros dominios
- **JWT**: Autenticación mediante tokens
- **Entity Framework**: Acceso a base de datos
- **AutoMapper**: Mapeo entre entidades y DTOs

---

## 📁 Carpetas Principales

### **Controllers** - Controladores HTTP

Definen los endpoints (rutas) disponibles en la API:

- `AuthController`: Login, registro y asignación de permisos
- `UserController`: Información del usuario actual
- `PostController`: Crear, ver y eliminar publicaciones
- `LikeController`: Dar y quitar "me gusta" en posts
- `ComentController`: Crear, ver y eliminar comentarios
- `ChatController`: Crear y ver conversaciones privadas
- `MessageController`: Enviar y recibir mensajes
- `GroupController`: Crear grupos y gestionar membresía

### **Entities** - Modelos de Base de Datos

Clases que representan las tablas en la capa de datos:

- `User`: Usuario registrado en el sistema
- `Post`: Publicación creada por un usuario
- `Like`: "Me gusta" en un post
- `Coments`: Comentario en un post
- `Chat`: Conversación privada entre dos usuarios
- `Message`: Mensaje dentro de un chat
- `Group`: Grupo al que los usuarios pueden unirse

### **DTOs** - Objetos de Transferencia de Datos

Clases intermediarias que envían/reciben datos con el cliente:

- **Auth**: DTOs para autenticación (login/registro)
- **User**: DTOs para información de usuario
- **Post**: DTOs para publicaciones
- **Coment**: DTOs para comentarios
- **Like**: DTOs para likes
- **ChatMessage**: DTOs para chats y mensajes
- **Group**: DTOs para grupos

### **Services** - Lógica de Negocio

Clases que contienen la lógica independiente del controlador:

- `IUserService` (interfaz): Contrato del servicio
- `UserService`: Obtiene el usuario autenticado actual

### **Data** - Contexto de Base de Datos

- `ApplicationDbContext`: Configura las relaciones entre entidades y la base de datos

### **Utils** - Utilidades

- `AutoMapperProfile`: Define cómo convertir entidades a DTOs y viceversa

---

## 🔐 Autenticación y Autorización

### Flujo de Autenticación

1. El usuario se registra con email, nombre de usuario y contraseña
2. Se crea una cuenta y se retorna un **token JWT**
3. El cliente guarda el token y lo envía en siguientes solicitudes
4. El servidor valida el token y permite acceso a recursos protegidos

### Tipos de Usuarios

- **Usuario normal**: Puede crear posts, comentar, dar likes y chatear
- **Administrador**: Además puede asignar permisos (mediante claim "admin")

---

## 🗄️ Relaciones entre Entidades

```
User (1) ──→ (N) Post
         ──→ (N) Like
         ──→ (N) Coments
         ──→ (N) Chat
         ──→ (N) Message
         ──→ (N) Groups

Post (1) ──→ (N) Like
     ──→ (N) Coments

Chat (1) ──→ (N) Message

Group (1) ──→ (N) Users
```

**Comportamientos de eliminación en cascada**:

- Si se elimina un Post → Se eliminan sus Likes y Comentarios
- Si se elimina un Chat → Se eliminan sus Mensajes
- No se permite eliminar un Usuario si tiene Likes o Comentarios

---

## 🛣️ Rutas de la API (Endpoints)

### **Autenticación** (`/v1/api/auth`)

- `POST /register` - Registrar nuevo usuario
- `POST /login` - Iniciar sesión
- `POST /admin` - Asignar permisos de administrador

### **Usuarios** (`/v1/api/users`)

- `GET /` - Obtener todos los usuarios (solo admin)
- `GET /me` - Obtener usuario actual
- `PUT /update` - Actualizar perfil del usuario actual

### **Posts** (`/v1/api/posts`)

- `GET /` - Obtener todos los posts
- `GET /{id}` - Obtener un post específico
- `POST /` - Crear un nuevo post
- `DELETE /{id}` - Eliminar un post (solo autor)

### **Likes** (`/v1/api/posts/{postId}/likes`)

- `POST /` - Dar like a un post
- `DELETE /{id}` - Quitar like de un post

### **Comentarios** (`/v1/api/posts/{postId}/coments`)

- `GET /` - Obtener comentarios de un post
- `POST /` - Crear comentario en un post
- `DELETE /{id}` - Eliminar comentario (solo autor)

### **Chats** (`/v1/api/chats`)

- `GET /` - Obtener conversaciones del usuario
- `GET /{chatId}` - Obtener detalles de un chat
- `POST /{userName}` - Iniciar conversación con usuario

### **Mensajes** (`/v1/api/chats/{chatId}/messages`)

- `GET /` - Obtener mensajes de un chat
- `POST /` - Enviar mensaje en un chat

### **Grupos** (`/v1/api/groups`)

- `GET /` - Obtener todos los grupos
- `GET /{id}` - Obtener detalles de un grupo
- `POST /` - Crear nuevo grupo
- `POST /{groupId}/join` - Unirse a un grupo
- `POST /{groupId}/leave` - Salir de un grupo
- `DELETE /{id}` - Eliminar un grupo

---

## 🔄 Flujo de una Solicitud Típica

1. **Cliente envía solicitud HTTP** con datos y token JWT
2. **Middleware valida el token** JWT y carga el usuario autenticado
3. **Controlador recibe la solicitud** y valida los datos
4. **Servicio ejecuta la lógica** de negocio
5. **Base de datos** es actualizada/consultada
6. **DTO es retornado al cliente** (no la entidad completa)

---

## 📦 Validaciones

### En la API

- Campos requeridos
- Longitudes mínimas y máximas de textos
- Formatos de email
- Validación de tokens JWT

### En Base de Datos

- Claves foráneas
- Restricciones de eliminación en cascada
- Valores únicos (email, nombres de usuario)

---

## 🛡️ Seguridad

- **Contraseñas**: Se encriptan antes de almacenarlas (ASP.NET Identity)
- **Tokens JWT**: Validan automáticamente en cada solicitud autenticada
- **CORS**: Controla qué dominios pueden acceder a la API
- **Autorización basada en claims**: Los permisos se verifican por claims en el token

---

## 📊 Tecnologías Utilizadas

- **Lenguaje**: C# (.NET 10.0)
- **Base de Datos**: SQL Server
- **ORM**: Entity Framework Core
- **Autenticación**: JWT + ASP.NET Identity
- **Mapeo de Objetos**: AutoMapper
- **Documentación API**: Swagger/OpenAPI

---

## 🚀 Para Empezar

1. Restaurar las migraciones: `dotnet ef database update`
2. Ejecutar la aplicación: `dotnet run`
3. Acceder a Swagger: `http://localhost:5000/swagger`
4. Registrar un usuario y copiar el token
5. Usar el token en el header: `Authorization: Bearer {token}`

---

## 📝 Notas Importantes

- Todos los endpoints excepto `/auth/register` y `/auth/login` requieren autenticación
- Los usuarios solo pueden eliminar sus propios posts y comentarios
- Un usuario no puede dar like múltiples veces al mismo post
- Los chats son privados entre dos usuarios específicos
- Los grupos tienen un máximo de miembros (configurable, por defecto 200)
