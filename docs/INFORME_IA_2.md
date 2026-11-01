# Auditoría UX/A11y – TaskFlow DAM (segunda pasada)

**Modelo de IA:** Claude (Anthropic)
**Prompt:** Prompt Maestro de Auditoría de Cátedra (apartado 5.2 del enunciado)
**Material revisado:** captura de la ventana (`docs/captura_final.png`), `MainWindow.xaml`, `App.xaml`, `MainWindow.xaml.cs` y el árbol UI Automation de la aplicación en ejecución.

---

## 1. Heurísticas de Nielsen

| Heurística | Resultado | Evidencia |
|---|---|---|
| H1 Visibilidad del estado | Cumple | `ProgressBar` enlazada a `ProgresoPorcentaje`; `StatusBar` con `EstadoApp` y `TareasPendientesCount`; `LiveSetting="Polite"` en el progreso y en los contadores |
| H3 Control y libertad | Cumple | Botón Limpiar con `IsCancel="True"` (Esc); confirmación Sí/No antes de eliminar |
| H4 Consistencia | Cumple | Colores y estilos centralizados en `App.xaml`; botones primario y secundario con el mismo formato; acción de eliminar siempre en rojo |
| H5 Prevención de errores | Cumple | Botón Añadir deshabilitado con el título vacío (`PuedeAnadir`); `DatePicker` con `DisplayDateStart`; validación de fecha en `BtnAgregar_Click`; `ComboBox` y `RadioButton` en vez de texto libre |
| H7 Flexibilidad y eficiencia | Cumple | `TabIndex` 1-9 en orden lógico; el orden visual de los botones coincide con el de tabulación; `IsDefault="True"`; mnemónicos Alt+T, M, F, P, B, E, A, U, L |
| H8 Estética y minimalismo | Cumple | Tres tarjetas separadas, márgenes de 10-15 px y paleta de 2-3 colores |

## 2. Accesibilidad y UI Automation

- **Name y HelpText:** comprobados en el árbol UIA en ejecución. Los 29 controles interactivos (campos, opciones, botones, grupo, lista, filas y botones de cada fila) tienen nombre y texto de ayuda. El botón interno del calendario se anuncia como "Abrir calendario".
- **Etiquetas:** `Label Target` en Título, Módulo, Fecha y Prioridad. La prioridad está agrupada en un `GroupBox` ("Prioridad de la tarea").
- **Lista:** es un `ListBox`, así que el lector anuncia "Lista de tareas" y cada fila como elemento de la lista.
- **Contraste (calculado con los colores de `App.xaml`):**

| Texto | Fondo | Ratio |
|---|---|---|
| `#1F2933` | `#FFFFFF` | 14.6:1 |
| `#444F5A` | `#FFFFFF` | 8.4:1 |
| `#FFFFFF` | `#0E5A7A` (botón) | 7.6:1 |
| `#444F5A` | `#E3E9EE` (botón deshabilitado) | 6.8:1 |
| Texto de badges | Fondo de badges | 8.1 - 8.9:1 |

Todos superan el 4.5:1 de WCAG 2.1 AA, y los textos activos superan 7:1 (AAA).

## 3. Ergonomía (ISO 9241-110 / Fitts / Gestalt)

- **Dianas:** botones Añadir y Limpiar de 44 px, campana y eliminar de 40 px, opciones y casillas con un mínimo de 36 px y clic en todo el control.
- **Región común:** formulario, progreso y lista están en tarjetas (`Border`), y la prioridad en un `GroupBox`.
- **Redimensionado:** la ventana mínima (650×500) no recorta controles; aparece una barra de desplazamiento.

## 4. Matriz de incidencias

| ID | Incidencia | Severidad | Vector |
|---|---|---|---|
| R-01 | El `ComboBox` mantiene el estilo gris con degradado de Windows, distinto del resto de campos, que son planos | 1 | H4 |
| R-02 | El `DatePicker` muestra la fecha como `2/11/2026` y la lista como `02/11/2026` | 1 | H4 |
| R-03 | No hay opción de deshacer al eliminar (está mitigado con la confirmación previa) | 1 | H3 |

No se detectan incidencias de severidad 2, 3 ni 4.

## 5. Puntuación

- **Test:** 9.1 / 10
- **Bloque de usabilidad estimado:** 5.0 − 3 × 0.15 = **4.55 / 5**
- **Dictamen:** apto. Solo quedan detalles cosméticos.
