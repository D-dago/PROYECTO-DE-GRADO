# Revisión del catálogo de carreras (2026-11-14, ORION #32)

Motivo: el resultado sugería el área «Agronomía y veterinaria», pero el catálogo no tenía ninguna carrera de esa área. Se revisó toda la oferta de las instituciones activas para encontrar las carreras que faltaban y las que ya no se dictan en Cochabamba.

## Método

- Fuente: la página oficial de cada institución o facultad, consultada el 2026-11-14. Solo se agrega una carrera si la página la asocia a Cochabamba (sede, campus o dirección).
- No se agregan carreras de sedes de provincia de la UMSS (Trópico, Valle Alto, Valle de Sacta, Conosur), porque el catálogo ubica a cada institución en un solo municipio.
- Si una institución no publica qué carreras dicta en su sede de Cochabamba, no se le agrega nada.
- Una carrera que ya no figura en la oferta oficial de Cochabamba se **oculta** (no se borra). Queda con estado `NO OFRECIDA EN COCHABAMBA` y el motivo en su fuente. El administrador puede reactivarla.
- Cada carrera nueva tiene estado `VERIFICADA`, su fuente con fecha y dos reglas de compatibilidad (90-80 para la dimensión principal y 45-70 para la secundaria, como las existentes). **Las reglas son una propuesta del sistema, no vienen de las instituciones: el autor de la tesis debe revisarlas.**
- Antes de cambiar nada se respaldaron las tablas en `respaldos/neon-careers-…-2026-11-14.json` y `respaldos/neon-compatibilities-…-2026-11-14.json` (y lo mismo para la base local).

## Resultado

| | Neon (internet) | Base local |
|---|---|---|
| Carreras agregadas | 59 (116 reglas) | 59 (116 reglas) |
| Carreras ocultadas | 10 | 10 |
| Carreras activas después (en instituciones activas) | 239 | 239 |

### Agregadas

| Institución | Carreras | Fuente |
|---|---|---|
| UMSS | Ingeniería Agronómica, Agroindustrial, Forestal y Ambiental (del Medio Ambiente) | [fcapyf.umss.edu.bo/carreras](https://fcapyf.umss.edu.bo/carreras/), Av. Petrolera km 5 |
| UMSS | Medicina Veterinaria y Zootecnia (se dicta en Quillacollo) | [registro oficial de carreras](https://websis.umss.edu.bo/umss_carreras.asp?codSer=UMSS), Facultad de Ciencias Veterinarias |
| UMSS | Biología, Ciencia Política | [umss.edu.bo/carreras](https://www.umss.edu.bo/carreras/) |
| UMSS | Física, Matemáticas, Química, Ingeniería Eléctrica, Mecánica, de Alimentos, Informática, Matemática, en Biotecnología y en Energía | [registro oficial de carreras](https://websis.umss.edu.bo/umss_carreras.asp?codSer=UMSS), Facultad de Ciencias y Tecnología |
| UMSS | Nutrición y Dietética, Odontología | [registro oficial de carreras](https://websis.umss.edu.bo/umss_carreras.asp?codSer=UMSS) |
| UMSS | Lingüística Aplicada a la Enseñanza de Lenguas, Trabajo Social, Ciencias de la Actividad Física y del Deporte, Música, Parvulario (técnico superior) | [hum.umss.edu.bo](https://www.hum.umss.edu.bo/) |
| UMSS | Planificación del Territorio y el Medio Ambiente, Turismo, Construcción Civil (técnico superior) | [fach.umss.edu.bo](https://fach.umss.edu.bo/) |
| UMSS | Sociología, Historia | [facso.umss.edu.bo](https://facso.umss.edu.bo/) |
| Univalle | Ingeniería en Ciencia de Datos e Inteligencia de Negocios, de Alimentos (Tecnología Alimentaria), Aeronáutica, en Energía, Mecánica y de Automatización Industrial | [Oferta Académica Cochabamba](https://www.univalle.edu/?page_id=20106) |
| UNITEPC | Medicina Veterinaria y Zootecnia, Fonoaudiología, Ingeniería de Sonido, Artes y Escultura, Prótesis Dental (técnico superior) | [unitepc.edu.bo/cochabamba](https://unitepc.edu.bo/cochabamba) |
| UPDS | Ingeniería en Gestión Petrolera | [sede Cochabamba](https://www.upds.edu.bo/sede/cochabamba/) |
| Salesiana | Ingeniería de Software | [usalesiana.edu.bo/cochabamba](https://www.usalesiana.edu.bo/cochabamba) |
| EMI | Ingeniería Agroindustrial, Ingeniería Civil; técnicos superiores en Diseño Gráfico, Electrónica (Sistemas Electrónicos), Sistemas Informáticos (Informática), Energías Renovables y Construcción Civil | [unidad Cochabamba](https://www.emi.edu.bo/universidad/unidades-academicas/cochabamba) |
| UCB | Diseño Digital Multimedia, Filosofía y Letras, Ingeniería Empresarial, Teología, Comunicación Multimedia (técnico superior) | [oferta de pregrado Cochabamba](https://cba.ucb.edu.bo/oferta-pregrado-ucb-cochabamba/) |
| USIP | Ingeniería Ambiental, Matemáticas; técnicos superiores en Software y en Administración y Negocios | [usip.edu.bo](https://usip.edu.bo/) (páginas de facultad y de técnico superior) |
| Unifranz | Auditoría Financiera y Control de Gestión, Diseño Digital y Producción Transmedia | página de cada carrera en [unifranz.edu.bo/carreras](https://unifranz.edu.bo/carreras/) (sedes: incluye Cochabamba) |

### Ocultadas (no se dictan en Cochabamba según la fuente oficial)

| Institución | Carrera | Motivo |
|---|---|---|
| Unifranz | Arquitectura | Su página solo lista la sede Santa Cruz |
| Unifranz | Comunicación Social | «Comunicación y Periodismo Digital» no incluye Cochabamba |
| Unifranz | Enfermería | Solo El Alto y Santa Cruz |
| Unifranz | Gastronomía | No incluye Cochabamba |
| Unifranz | Turismo y Hotelería | No incluye Cochabamba |
| UPDS | Ingeniería Civil, Marketing y Publicidad, Turismo y Hotelería | No figuran en la página de la sede Cochabamba (estaban como «confirmar sede») |
| UCB | Ingeniería en Telecomunicaciones | No figura en la oferta de pregrado de Cochabamba (actualizada el 2026-10-15) |
| Univalle | Ingeniería Mecatrónica | No figura en la Oferta Académica Cochabamba |

### Sin cambios

- **UPAL e INFOCAL:** su oferta oficial coincide con el catálogo.
- **UPB:** su sitio no indica en qué campus se dicta cada programa; sus carreras siguen como «confirmar sede».
- **TECBA:** tiene sedes en Cochabamba y Sucre y su menú no separa la oferta por sede.
- **ULAT:** su sitio solo muestra tres carreras destacadas, sin la lista completa.
- **INCOS N.° 3 (Quillacollo):** no tiene sitio oficial.
- **Instituciones inactivas:** UPEA e Instituto Puerto de Mejillones están en El Alto (La Paz) e Instituto Álvarez Plata no tiene sitio confirmado. Siguen ocultas.

## Pendiente

- Revisar las reglas de compatibilidad propuestas (dimensión y nivel) de las 59 carreras nuevas.
- Las carreras de la UMSS tomadas de su registro oficial (websis) no tienen página propia de la facultad que las confirme; conviene confirmarlas en la próxima convocatoria de admisión.
- Confirmar la sede de las carreras de UPB y TECBA, y la lista completa de ULAT.
