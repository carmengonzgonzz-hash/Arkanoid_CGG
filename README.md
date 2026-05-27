# ARKANOID #
``Por Carmen González y Marcos Benavente``

**Proyecto final del 3º Trimestre RPMI**

Nosotros implementamos la mejora del estado pausa al flujo de juego.


## 1. Añadimos el nuevo estado: ##

Lo primero fue ampliar el enum "GameState" añadiendo un nuevo estado (Pause) al ``GameManager``.
```csharp
public enum GameState
{
    Menu,
    Playing,
    Pause,
    Victory,
    GameOver
}
```
Con esto el juego puede detectar cuándo está pausado igual que detecta el resto de estados del juego.


## 2. Crear el PanelPause en la interfaz: ##

Creamos en Unity un nuevo panel UI llamado "PanelPause" para mostrar el menú de pausa.

Después se añadió una referencia serializada al ``GameManager``:
```csharp
[SerializeField] GameObject panelPause;
```
Para poder asignar el panel desde el Inspector de Unity.


## 3. Mostrar el panel según el estado: ##

Dentro del método "SetState()" del ``GameManager`` añadiremos el método "SetActive()" que permite encender o apagar GameObjects de la escena:
```csharp
panelPause.SetActive(newState == GameState.Pause);
```
Con esto el panel aparece cuando el estado es Pause y desaparece automáticamente en cualquier otro estado.


## 4. Detectar la tecla ESC para pausar: ##

Dentro del método Update() del ``GameManager`` añadimos la detección de la tecla Escape.
```csharp
 void Update()
if (Input.GetKeyDown(KeyCode.Escape))
{
    if (state == GameState.Playing)
    {
        SetState(GameState.Pause);
    }
    else if (state == GameState.Pause)
    {
        ResetBall();
        SetState(GameState.Playing);
    }
}
```
Este sistema permite pasar de Playing a Pause y volver de Pause a Playing


## 5. Congelar y reanudar el tiempo: ##

La pausa se implementó utilizando time.TimeScale dentro del SetState() de ``GameManager``.
```csharp
if (newState == GameState.Pause)
{
    Time.timeScale = 0f;
}
else
{
    Time.timeScale = 1f;
}
```
Con esto la física se detiene, el movimiento se congela, el juego queda completamente pausado y al volver a Playing el tiempo vuelve a la normalidad.


## 6. Problema detectado con la pelota: ##

Durante las pruebas apareció un error, la pelota dejaba de moverse al quitar la pausa. El problema estaba en esta línea del ``BallController`` dentro del método Update():
```csharp
rb.simulated = false;
```
El Rigidbody2D de la pelota se desactivaba completamente al salir del estado Playing.


## 7. Corrección del Rigidbody de la pelota: ##

La solución fue modificar el sistema de simulación física en el Update() de ``BallController`` [Código completo de Update].
```csharp
void Update()
{
    // SOLO desactivar física si NO estamos jugando NI en pausa
    rb.simulated =
        gameManager.CurrentState == GameState.Playing ||
        gameManager.CurrentState == GameState.Pause;

    // Bola pegada a la pala
    if (isAttached && paddleTransform != null)
    {
        transform.position = paddleTransform.position + (Vector3)attachOffset;

        if (gameManager.CurrentState == GameState.Playing &&
            Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }
}
```
Con este cambio, el Rigidbody permanece activo en pausa el Time.timeScale = 0 se encarga de congelar la física y al reanudar, la pelota continúa moviéndose correctamente.


## 8. Botón Resume: ##

En Unity crearemos un botón que será nuestro Resume dentro del menú de Pausa en dicha interfaz, por ello añadimos un método en el ``GameManager`` para el botón Resume del menú de pausa.
```csharp
public void ResumeGame()
{
    SetState(GameState.Playing);
}
```
Este método se vinculó desde el componente Button de Unity.


## 9. Botón Return To Menu: ##

También se añadió el método ReturnToMenu() al ``GameManager``.
```csharp
public void ReturnToMenu()
{
    SetState(GameState.Menu);
    ResetBall();
}
```
Este botón vuelve al menú principal, resetea la pelota y reinicia correctamente el flujo del juego.
