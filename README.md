# 🔐 HollowKeyauht – Cliente de Escritorio para HollowAuth

**HollowKeyauht** es una aplicación de escritorio desarrollada en **C# (.NET Framework 4.7.2)** que sirve como cliente de ejemplo/integración para la plataforma de autenticación y gestión de licencias **HollowAuth**.

Esta aplicación demuestra cómo implementar un sistema completo de login, registro y validación de licencias usando la API de HollowAuth, con un enfoque en la protección de software mediante **HWID (Hardware ID)** y gestión de suscripciones.

## ✨ Características Principales

*   **Autenticación de Usuarios:** Permite iniciar sesión con un nombre de usuario y contraseña validados contra el servidor de HollowAuth.
*   **Registro de Nuevos Usuarios:** Los usuarios pueden registrarse dentro de la aplicación, vinculando su cuenta a una licencia válida.
*   **Login por Licencia:** Acceso directo utilizando únicamente una clave de licencia, ideal para usuarios que no quieren crear una cuenta.
*   **Protección por HWID:** Enlaza la cuenta o licencia al hardware del equipo, evitando el uso compartido de credenciales.
*   **Información de Suscripción:** Muestra al usuario el tipo de plan (ej. Gratis, Plus) y la fecha de expiración de su suscripción.
*   **Variables Remotas:** Prepara la aplicación para recibir configuraciones o anuncios directamente desde la web sin necesidad de actualizar el software.
*   **Interfaz Amigable:** Diseño limpio y responsivo con indicadores de estado en tiempo real.

## 🛠️ Tecnologías Utilizadas

*   **Lenguaje:** C#
*   **Framework:** .NET Framework 4.7.2
*   **UI:** Windows Forms
*   **Dependencias Principales:**
    *   `Newtonsoft.Json`: Para el manejo de datos JSON en las peticiones a la API.
    *   `System.Management`: Para la obtención del HWID único del equipo.

## 🚀 Cómo Funciona

1.  La aplicación se conecta al servidor de HollowAuth mediante su **API**.
2.  Al iniciar, comprueba la conexión y la validez de las credenciales de la app (App ID y Secret).
3.  El usuario puede:
    *   **Iniciar sesión:** Envía usuario/contraseña + HWID al servidor.
    *   **Registrarse:** Envía usuario/contraseña + Licencia + HWID al servidor.
    *   **Usar Licencia:** Envía solo la clave de licencia + HWID.
4.  El servidor valida la información, verifica el HWID y la fecha de expiración.
5.  Si es exitoso, el servidor devuelve los datos del usuario y la aplicación permite el acceso al formulario principal (`Form2`).

## 📷 Vista Previa

*Interfaz principal de inicio de sesión.*

## 🗂️ Estructura del Proyecto (Código)

*   `Form1.cs` / `Form1.Designer.cs`: Contiene la lógica y el diseño de la interfaz de login/registro.
*   `Form2.cs` / `Form2.Designer.cs`: El formulario principal al que se accede tras una autenticación exitosa.
*   `HollowAuth.cs`: La clase que envuelve toda la lógica de comunicación con la API de HollowAuth (métodos `login`, `register`, `license`, `init`).
*   `Program.cs`: El punto de entrada de la aplicación.

## ⚙️ Configuración Inicial

Para usar este proyecto, debes configurar tus credenciales en el archivo `Form1.cs`:

```csharp
// HollowKeyauht/Form1.cs

public HollowAuth.api HollowApp = new HollowAuth.api(
    "NOMBRE_DE_TU_APP",        // El nombre de tu aplicación en HollowAuth
    "TU_APP_ID",                // El App ID de tu dashboard
    "TU_SECRET",                // El Secret de tu dashboard
    version: "1.0",
    apiUrl: "https://hollowauth.onrender.com/api/client" // URL de la API (generalmente no cambia)
);
```

## ▶️ Cómo Ejecutar

1.  Clona el repositorio.
2.  Abre el archivo `HollowKeyauht.sln` con **Visual Studio 2019 o superior**.
3.  Restaura los paquetes de NuGet (Newtonsoft.Json, System.CodeDom, System.Management).
4.  Configura tus credenciales como se indica arriba.
5.  Presiona `F5` para compilar y ejecutar.

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Si encuentras un error o tienes una mejora, por favor, abre un **Issue** o un **Pull Request**.

## 📜 Licencia

Este proyecto se distribuye bajo la licencia **MIT**. Consulta el archivo `LICENSE` para más información.

## 🙏 Agradecimientos

*   A **[xShakul](https://github.com/xShakul)** por crear y mantener la plataforma **[HollowAuth](https://hollowauth.onrender.com/)**.
*   A la comunidad open-source por las herramientas que hacen esto posible.
