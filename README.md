# TP Movimiento, Recolección y UI — Zubek

Prototipo 3D en Unity 6.5 (URP). El jugador recorre un nivel hecho con ProBuilder y tiene que juntar todos los cangrejos para ganar.

**Video de gameplay:** _pegar acá el enlace de YouTube_

## Controles

| Tecla | Acción |
| --- | --- |
| W A S D / flechas | Moverse (relativo a la cámara) |
| Espacio | Saltar (solo en el suelo) |
| Clic derecho + mouse | Girar la cámara |
| Ruedita del mouse | Acercar / alejar la cámara |
| R | Reiniciar después de ganar |

## Cómo se cumple cada requisito

| Requisito | Dónde |
| --- | --- |
| Jugador con Rigidbody y AddForce | `PlayerController.cs` → `FixedUpdate()` |
| Movimiento con Input.GetAxis | `PlayerController.cs` → `Update()` |
| Salto con ForceMode.Impulse | `PlayerController.cs` → `FixedUpdate()` |
| Salto solo en el suelo (Tag Ground, isGrounded, OnCollisionEnter/Exit) | `PlayerController.cs` |
| Nivel con Grayboxing y ProBuilder (pisos, paredes, rampas, plataformas, obstáculos) | Escena `SampleScene`, objeto `Nivel` |
| Prefab de item coleccionable con Tag | `Assets/Prefabs/Item.prefab`, Tag `Item` |
| Trigger + OnTriggerEnter | Sphere Collider (Is Trigger) del Item + `PlayerController.OnTriggerEnter` |
| El item se destruye y suma al contador | `Destroy()` + `GameManager.SumarItem()` |
| UI con TextMeshPro ("CANGREJOS: 0 / 3") | `Canvas` → `TextoCangrejo` |
| Condición y panel de victoria | `GameManager.Ganar()` → panel de victoria |
| Cámara que sigue al jugador | `CameraFollow.cs` en Main Camera |

## Scripts

- `PlayerController.cs`: movimiento, salto, detección de suelo y recolección.
- `GameManager.cs`: contador, UI, victoria y reinicio.
- `CameraFollow.cs`: cámara en tercera persona que sigue al jugador y se puede girar con el mouse.
- `ItemRotator.cs`: hace girar los coleccionables.

## Créditos

Modelos 3D (personaje y cangrejo) de [Kenney](https://kenney.nl), licencia CC0.
