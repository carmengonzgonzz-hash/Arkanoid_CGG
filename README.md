# ARKANOID #
``Por Carmen González y Marcos Benavente``
```
Proyecto final del 3º Trimestre RPMI
```
Nosotros implementamos la mejora del estado pausa al flujo de juego.

Agregando:
Lo primero fue ampliar el enum GameState añadiendo un nuevo estado (Pause):
```
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
