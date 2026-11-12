# "Continuar con Google" con Firebase (plan gratis)

El botón **Continuar con Google** usa [Firebase Authentication](https://firebase.google.com/?hl=es-419) en el plan gratuito *Spark*. Iniciar sesión con Google no tiene costo en ese plan.

## Cómo funciona

1. El estudiante pulsa el botón y elige su cuenta en la ventana de Google. La página descarga Firebase solo en ese momento.
2. Firebase entrega un *ID token* firmado por Google.
3. El servidor lo comprueba por su cuenta, sin secretos:
   - la firma, con las claves públicas de Google;
   - que el token sea de **nuestro** proyecto;
   - que esté vigente;
   - que el correo esté verificado.
4. Si el correo ya tiene cuenta, entra a ella. Si no, se crea una cuenta de estudiante.

## Configurar (una sola vez)

1. Entrar a <https://console.firebase.google.com> con la cuenta de Google del proyecto y pulsar **Crear un proyecto**. Puede llamarse `orientacion-vocacional`. Google Analytics no hace falta.
2. En el menú: **Compilación → Authentication → Comenzar**. Luego, en **Método de acceso**, elegir **Google**, activarlo, elegir el correo de asistencia y **Guardar**.
3. En **Authentication → Configuración → Dominios autorizados → Agregar dominio**, escribir `orientacion-vocacional-nrzk.onrender.com`. `localhost` ya viene incluido.
4. En **Configuración del proyecto** (el engranaje) → **Tus apps** → icono web `</>`, poner un nombre y pulsar **Registrar app**. Firebase muestra un bloque `firebaseConfig`; se necesitan `apiKey`, `authDomain`, `projectId` y `appId`.
5. Esos cuatro datos van en la sección `Firebase` de `backend/OrientacionVocacional.API/appsettings.json`, o como variables `Firebase__ProjectId`, `Firebase__ApiKey`, `Firebase__AuthDomain` y `Firebase__AppId` en Render.

   Estos datos **no son secretos**: Firebase los publica en cada página que lo usa. Lo que impide que otro sitio use el proyecto es la lista de **dominios autorizados** del paso 3.

## Problemas comunes

| Mensaje | Solución |
| --- | --- |
| "todavía no está configurado" | Faltan los datos del paso 5. |
| "Esta dirección no está autorizada en Firebase" | Agregar el dominio en el paso 3. |
| "no está activado en Firebase" | Activar Google en el paso 2. |
| La ventana de Google no se abre | El navegador bloqueó la ventana emergente. El sistema cambia solo a redirección. |
