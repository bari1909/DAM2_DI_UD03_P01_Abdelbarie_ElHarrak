Auditoría Senior UX/A11y — TaskFlow DAM
Conforme parcialmente
ISO 9241-110
WCAG 2.1 AA

Alcance de la auditoría

Se han examinado los dos elementos aportados:
1. Captura de pantalla de TaskFlow DAM (1216 × 815 px).
2. Archivo MainWindow.xaml, incluyendo estructura, controles, propiedades, enlaces de datos, navegación por teclado y propiedades de UI Automation.
La evaluación distingue entre:
- Verificado: existe evidencia directa en XAML o en la captura.
- Parcial: existe implementación, pero no se puede garantizar su comportamiento.
- No verificable: depende de recursos externos, código C# o ejecución de la aplicación.
Criterio de evaluación: no se considera que una propiedad esté correctamente implementada únicamente porque aparezca declarada en XAML. Debe poder demostrarse su funcionamiento.

1. Auditoría de las heurísticas de Nielsen (H1-H10)
Se evalúan especialmente las seis heurísticas solicitadas.
H1. Visibilidad del estado del sistema
Cumplimiento alto
Evidencias positivas:
- Existe un ProgressBar enlazado mediante Value="{Binding ProgresoPorcentaje, Mode=OneWay}".
- Se utiliza TextoProgreso para mostrar el porcentaje y el número de tareas completadas.
- La barra de estado (StatusBar) presenta:
  - Mensajes contextuales mediante EstadoApp.
  - Contador de tareas pendientes mediante TareasPendientesCount.
- La interfaz proporciona información visual sobre las tareas completadas mediante tachado.
- Se utiliza AutomationProperties.LiveSetting="Polite" en el mensaje de estado.
Incidencias:
- No se puede verificar que ProgresoPorcentaje se actualice correctamente ante cada modificación.
- El contador de tareas pendientes no tiene configurada explícitamente una región viva de accesibilidad.
- No se observa una notificación accesible específica cuando cambia el progreso.
Recomendación:

Implementar notificaciones accesibles cuando se complete una tarea y verificar que el ViewModel notifique los cambios mediante INotifyPropertyChanged.
H3. Control y libertad del usuario
Cumplimiento alto
Evidencias positivas:

El botón Limpiar incorpora:
<Button Content="_Limpiar (Esc)"
        IsCancel="True"
        Click="BtnLimpiar_Click"/>


Esto proporciona:
- Una acción explícita para restablecer el formulario.
- Un mecanismo de cancelación mediante Escape.
- Un mnemónico mediante el carácter _.
También existe un botón independiente para eliminar tareas.

Incidencias:
- No se puede verificar si BtnLimpiar_Click restablece todos los campos correctamente.
- La eliminación de tareas requiere especial atención: aunque el HelpText indica que se solicitará confirmación, no se ha aportado el código C# para demostrarlo.
- No existe una acción visible de deshacer una eliminación.
Recomendación:

Incorporar confirmación antes de eliminar y, preferiblemente, una opción para deshacer la operación.
H4. Consistencia y estándares visuales
Cumplimiento alto
Evidencias positivas:
- Se utiliza Segoe UI como tipografía general.
- Existe una jerarquía visual reconocible.
- Los controles mantienen una distribución consistente.
- Se emplean tarjetas mediante Border.
- Las prioridades se representan mediante indicadores cromáticos.
- Los márgenes y separaciones mantienen una distribución relativamente uniforme.
Incidencias:
- La prioridad se identifica mediante colores, lo que puede generar problemas si estos son la única señal visual.
- Los iconos utilizan caracteres de Segoe MDL2 Assets, cuya representación depende de la fuente instalada.
- No se puede verificar la consistencia completa de los estilos sin las definiciones de recursos externos.
Recomendación:

Mantener las etiquetas textuales Baja, Media y Alta, sin depender exclusivamente del color, y utilizar iconos con alternativas textuales accesibles.
H5. Prevención de errores
Cumplimiento alto
Evidencias positivas:
<TextBox MaxLength="200"
         Text="{Binding NuevoTitulo,
         Mode=TwoWay,
         UpdateSourceTrigger=PropertyChanged}"/>


- Se establece un máximo de 200 caracteres.
- El botón Añadir está condicionado por PuedeAnadir.
- Se utiliza un ComboBox para seleccionar módulos predefinidos.
- Se utiliza DatePicker para seleccionar fechas.
- Se restringe la fecha mínima mediante DisplayDateStart.
- La prioridad se selecciona mediante RadioButton agrupados.
- El título se identifica como campo obligatorio.
Incidencias:
- DisplayDateStart restringe la selección del calendario, pero no demuestra que se rechacen fechas inválidas introducidas manualmente.
- No se observa un indicador visual de error asociado al campo del título.
- No se puede verificar la validación de fechas ni la gestión de excepciones en el código C#.
Recomendación:

Implementar validaciones mediante INotifyDataErrorInfo o IDataErrorInfo, con mensajes de error asociados al control correspondiente.
H7. Flexibilidad y eficiencia de uso
Cumplimiento parcial
Evidencias positivas:
- IsDefault="True" en el botón Añadir.
- IsCancel="True" en el botón Limpiar.
- Se utilizan mnemónicos mediante _.
- Existe configuración explícita de TabIndex.
- El foco inicial se establece en TxtTitulo.
Incidencia detectada: orden de navegación incorrecto.

En el XAML:
<Button Content="_Limpiar (Esc)"
        TabIndex="9"/>

<Button x:Name="BtnAgregar"
        TabIndex="8"/>


Visualmente, Limpiar aparece antes que Añadir, pero el orden de tabulación invierte ambos controles.

Esto puede generar una navegación poco predecible mediante teclado.

Recomendación:

Establecer el siguiente orden:
Control	TabIndex
Título	1
Módulo	2
Fecha límite	3
Prioridad baja	4
Prioridad media	5
Prioridad alta	6
Urgente	7
Limpiar	8
Añadir	9
Notificaciones	10

También se recomienda comprobar el comportamiento de los mnemónicos cuando existen varias ventanas o controles con letras repetidas.
H8. Estética y diseño minimalista
Cumplimiento alto
Evidencias positivas:
- Interfaz limpia y organizada.
- Uso de espacios en blanco.
- Separación de funcionalidades mediante tarjetas.
- Ausencia de elementos decorativos excesivos.
- Jerarquía tipográfica clara.
- Distribución reconocible entre formulario, progreso y tareas activas.
Incidencias:
- La interfaz presenta bastantes indicadores simultáneos: prioridad, módulo, urgencia, fecha y eliminación.
- Los iconos podrían generar problemas de comprensión sin etiquetas accesibles.
- La animación del botón Añadir puede resultar innecesaria para determinados usuarios.
Recomendación:

Respetar las preferencias de movimiento reducido y mantener una separación uniforme de 10-15 px entre elementos relacionados.
Resumen de heurísticas
Heurística	Resultado	Observación
H1. Visibilidad	Alto	Progreso y estado presentes
H2. Correspondencia	Alto	Terminología comprensible
H3. Control y libertad	Alto	Cancelación implementada
H4. Consistencia	Alto	Estilos coherentes
H5. Prevención de errores	Alto	Validación parcial verificable
H6. Reconocimiento	Alto	Opciones visibles y etiquetas
H7. Flexibilidad	Parcial	Orden de tabulación incorrecto
H8. Minimalismo	Alto	Diseño despejado
H9. Recuperación de errores	Parcial	Confirmación y recuperación no verificadas
H10. Ayuda y documentación	Parcial	ToolTips y HelpText presentes, ayuda contextual limitada


2. Auditoría de accesibilidad y UI Automation (WCAG 2.1 AA)
Cumplimiento parcial — requiere validación
2.1. AutomationProperties.Name y HelpText
Se ha revisado la presencia de propiedades de accesibilidad en los controles interactivos.
Control	Name	HelpText	Evaluación
TxtTitulo	Sí	Sí	Correcto
CmbModulo	Sí	Sí	Correcto
DpFechaLimite	Sí	Sí	Correcto
RbBaja	Sí	Sí	Correcto
RbMedia	Sí	Sí	Correcto
RbAlta	Sí	Sí	Correcto
CheckBox Urgente	Sí	Sí	Correcto
BtnLimpiar	Sí	Sí	Correcto
BtnAgregar	Sí	Sí	Correcto
BtnNotificaciones	Sí	Sí	Correcto
CheckBox de tareas	Sí	Sí	Correcto
Botón eliminar	Sí	Sí	Correcto
ProgressBar	Sí	Sí	Correcto

Resultado: los controles interactivos identificados cuentan con ambas propiedades explícitamente declaradas.

Sin embargo, hay dos aspectos que deben comprobarse:
1. Que los nombres se expongan correctamente a través del árbol UI Automation.
2. Que los lectores de pantalla anuncien los cambios de estado, especialmente al completar o eliminar tareas.
La presencia de las propiedades no garantiza por sí sola una experiencia accesible.
2.2. Vinculación de etiquetas: Label Target / LabeledBy
Se identifican estas vinculaciones:
<Label Content="_Título de la tarea:"
       Target="{Binding ElementName=TxtTitulo}"/>


<Label Content="_Módulo formativo:"
       Target="{Binding ElementName=CmbModulo}"/>


<Label Content="_Fecha límite:"
       Target="{Binding ElementName=DpFechaLimite}"/>


<Label Content="_Prioridad:"
       Target="{Binding ElementName=RbBaja}"/>


Evaluación:
- Título: vinculación correcta.
- Módulo: vinculación correcta.
- Fecha: vinculación correcta.
- Prioridad: vinculación directa únicamente con el primer RadioButton.
- Urgente: dispone de texto asociado mediante CheckBox.Content, aunque no utiliza un Label.Target explícito.
Incidencia: el grupo de prioridad debería disponer de una agrupación semántica propia, no depender exclusivamente de la etiqueta asociada al primer RadioButton.

Recomendación:

Utilizar GroupBox para agrupar las opciones de prioridad y proporcionar una etiqueta descriptiva común.
2.3. Contraste cromático WCAG 2.1 AA
El estándar WCAG 2.1 establece los siguientes requisitos:
Elemento	Contraste mínimo
Texto normal	4,5:1
Texto grande	3:1
Componentes de interfaz e información gráfica significativa	3:1

Observaciones sobre la captura:
- El texto principal presenta una buena diferenciación respecto al fondo.
- Los textos secundarios utilizan tonalidades más claras.
- Los indicadores de prioridad presentan fondos coloreados.
- Las etiquetas de módulo utilizan fondos azulados.
- La barra de progreso utiliza una combinación de azul y gris claro.
No obstante, el archivo aportado no contiene las definiciones de los recursos cromáticos (BrushTexto, BrushTextoSecundario, BrushPrimario, BrushBadgeAltaFondo, etc.).

Por tanto, no es técnicamente correcto certificar los ratios de contraste a partir de esta captura.

Es necesario disponer de App.xaml o del diccionario de recursos correspondiente para calcular los valores exactos.

La comprobación debe incluir también los estados deshabilitado, seleccionado, hover y focus.

3. Ergonomía de interacción: ISO 9241-110, Fitts y Gestalt
Cumplimiento parcial
3.1. Tamaño de las dianas interactivas
Como criterio ergonómico de esta auditoría se establece una dimensión objetivo de 36-44 píxeles para los controles interactivos, aunque no debe confundirse con un requisito universal de WCAG 2.1 AA.

La captura permite observar:
Elemento	Evaluación visual
Botón Añadir tarea	Adecuado
Botón Limpiar	Adecuado
Botón Notificaciones	Aceptable
Botones de eliminación	Posible incumplimiento
RadioButton de prioridad	Diana aparentemente reducida
CheckBox Urgente	Diana aparentemente reducida
CheckBox de tareas	Diana aparentemente reducida

Incidencia principal:

Los botones de eliminación y las casillas de selección presentan áreas visuales pequeñas.

Esto incrementa la dificultad de interacción, especialmente para usuarios con movilidad reducida.

La ley de Fitts establece que la dificultad de adquisición de un objetivo aumenta cuando disminuye su tamaño o aumenta la distancia hasta él.

Recomendación:
- Establecer alturas mínimas de 36-44 unidades independientes de WPF para botones principales.
- Ampliar el área interactiva de casillas y botones de icono.
- Mantener separación suficiente entre objetivos próximos.
- Evitar que el área de interacción dependa exclusivamente del tamaño del icono.
Importante: el tamaño real de los controles depende de los estilos externos EstiloBotonEliminar, EstiloBotonIcono y de la configuración de los controles. La captura permite detectar riesgos, pero no certificar sus dimensiones físicas.
3.2. Agrupación semántica: Gestalt
La aplicación utiliza Border para establecer regiones visuales.





Aspectos positivos:
- El formulario de creación constituye una región visual diferenciada.
- El progreso del día dispone de una tarjeta independiente.
- Las tareas activas se encuentran agrupadas dentro de otra región.
- Existe separación visual entre las funcionalidades.
Se cumple razonablemente el principio Gestalt de región común.

Aspectos mejorables:
- El grupo de prioridad está contenido en un StackPanel, sin una agrupación semántica explícita.
- El listado utiliza ItemsControl, que no ofrece por sí mismo la misma semántica de selección y navegación que un control especializado como ListBox.
- No se ha definido explícitamente una estructura accesible para las secciones principales.
Recomendaciones:
1. Utilizar GroupBox para la prioridad.
2. Valorar ListBox con un ItemTemplate personalizado para las tareas.
3. Mantener los Border como agrupación visual.
4. Comprobar el árbol UIA con Accessibility Insights o Inspect.
3.3. Evaluación según ISO 9241-110
Principio	Evaluación	Justificación
Adecuación a la tarea	Buena	La creación y gestión de tareas es directa
Autodescripción	Buena	Etiquetas y ToolTips explicativos
Conformidad con expectativas	Buena	Controles WPF convencionales
Adecuación al aprendizaje	Buena	Interfaz sencilla y predecible
Controlabilidad	Buena	Limpiar, añadir y completar tareas
Tolerancia a errores	Parcial	Validaciones adicionales necesarias
Capacidad de individualización	Parcial	No se observan preferencias de interfaz o movimiento


4. Matriz de incidencias por severidad
Se aplica la escala de severidad de Nielsen de 0 a 4:
Nivel	Clasificación	Significado
0	Sin incidencia	No existe problema de usabilidad
1	Cosmético	Defecto visual menor
2	Menor	Problema que dificulta parcialmente el uso
3	Grave	Problema importante que afecta a determinados usuarios o tareas
4	Catastrófico	Impide completar una tarea esencial

4.1. Incidencias identificadas
ID	Incidencia	Severidad	Vector
INC-01	Orden de TabIndex invertido entre Limpiar y Añadir	2	Nielsen H7
INC-02	Diana interactiva reducida en botones de eliminación	3	Ergonomía
INC-03	Diana interactiva reducida en casillas y opciones de prioridad	2	Ergonomía
INC-04	Falta de agrupación semántica explícita de prioridad	2	UIA / Gestalt
INC-05	No se acredita la actualización accesible del contador y progreso	2	WCAG / H1
INC-06	Falta de mecanismo de deshacer eliminación	2	Nielsen H3
INC-07	Validación de fechas introducidas manualmente no demostrada	2	Nielsen H5
INC-08	Contraste de colores no verificable sin recursos externos	Pendiente	WCAG 2.1
INC-09	Confirmación de eliminación no verificable sin código C#	Pendiente	Nielsen H3
INC-10	Navegación y semántica de la colección de tareas mejorables	2	UIA

Las incidencias pendientes no representan incumplimientos confirmados: requieren evidencia adicional.
4.2. Análisis de severidad 4: problemas catastróficos
No se identifican incidencias confirmadas de severidad 4
En la evidencia examinada no se detectan bloqueos absolutos que impidan utilizar las funcionalidades principales.

La aplicación dispone de:
- Campo de introducción de tareas.
- Selector de módulos.
- Selector de fechas.
- Selección de prioridades.
- Registro de tareas.
- Marcado de tareas completadas.
- Eliminación de tareas.
- Indicador de progreso.
La navegación por teclado también dispone de mecanismos explícitos de acceso.

Esto no equivale a certificar la ausencia absoluta de problemas catastróficos: sería necesario realizar pruebas funcionales y de accesibilidad con lectores de pantalla y navegación exclusiva mediante teclado.

5. Plan de corrección priorizado
Se recomienda abordar las siguientes mejoras en este orden:
Prioridad 1 — Ergonomía
Alta

Ampliar las áreas interactivas de eliminación, selección y prioridad hasta alcanzar dimensiones ergonómicas adecuadas.

Prioridad 2 — Navegación
Media-alta

Corregir el orden de TabIndex y verificar el recorrido completo mediante teclado.

Prioridad 3 — Semántica UIA
Media

Introducir GroupBox para prioridad y mejorar la semántica accesible del listado.

Prioridad 4 — WCAG
Media

Auditar los recursos cromáticos y verificar los ratios de contraste mediante herramientas automáticas.

Prioridad 5 — Prevención y recuperación
Mejora

Incorporar validaciones explícitas, confirmación de eliminación y posibilidad de deshacer.

6. Dictamen final del auditor
Aprobación condicionada
TaskFlow DAM presenta una base de diseño sólida, pero necesita mejoras antes de considerarse plenamente conforme con los criterios de accesibilidad y ergonomía establecidos.

La estructura visual, la distribución de controles, los mecanismos de navegación y las propiedades de automatización muestran una implementación consciente de los principios de usabilidad.

Sin embargo, existen aspectos pendientes relacionados con las dianas interactivas, la navegación mediante teclado, la agrupación semántica y la comprobación objetiva del contraste.

Valoración global
Área	Resultado
Heurísticas de Nielsen	Favorable con mejoras
ISO 9241-110	Favorable con mejoras
WCAG 2.1 AA	Conformidad no demostrada
UI Automation	Buena implementación declarativa
Ergonomía	Mejorable
Severidad 4	Sin incidencias confirmadas

Conclusión profesional:

TaskFlow DAM cumple razonablemente los principios fundamentales de usabilidad de una aplicación de escritorio WPF. Destaca especialmente la autodescripción de los controles, el uso de etiquetas, la presencia de estados visibles y la implementación de accesos rápidos.

No obstante, no debe declararse todavía una conformidad WCAG 2.1 AA completa, ya que faltan las verificaciones de contraste, árbol de accesibilidad, dimensiones reales de interacción y funcionamiento con tecnologías de asistencia.

7. Evidencias adicionales necesarias para cerrar la auditoría
Para emitir un dictamen definitivo sería necesario examinar:
1. App.xaml: comprobar los colores y estilos y calcular los ratios de contraste.
2. MainWindow.xaml.cs: verificar validaciones, confirmaciones, eventos y comportamiento de Escape.
3. ViewModel: comprobar actualización reactiva del progreso y los contadores.
4. Pruebas con NVDA y Accessibility Insights: verificar UI Automation y navegación accesible.
5. Pruebas de teclado: comprobar todos los recorridos de foco.
Herramientas recomendadas:
- Accessibility Insights for Windows.
- NVDA Screen Reader.
- Colour Contrast Analyser.
Con estos elementos se podría transformar esta evaluación documental en una auditoría de conformidad técnica mucho más concluyente.