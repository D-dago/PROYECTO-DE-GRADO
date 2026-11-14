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

## Segunda revisión: lo oculto y lo no confirmado (ORION #33)

Pedido: mostrar lo oculto que sea de Cochabamba y esté vigente. Se revisó cada institución y carrera oculta, y también las visibles con fuente dudosa. Plan aplicado: `tools/catalogo/plan-33.json` (carpeta local, no se sube a git).

**Se reactivó:** nada. Ningún elemento oculto se confirmó en Cochabamba con una fuente oficial actual:

| Oculto | Decisión | Motivo |
|---|---|---|
| UPEA, Instituto Puerto de Mejillones | Siguen ocultos | Están en El Alto (La Paz) |
| Instituto Álvarez Plata (Instituto Técnico Nacional de Comercio «Federico Álvarez Plata») | Sigue oculto | Existe en Cochabamba, pero no tiene sitio oficial y su única lista de carreras es un [plan de mejora de 2022](https://ftpnacional.cemse.edu.bo/wp-content/uploads/2022/10/PLAN-DE-MEJORA_ALVAREZ-PLATA-DIURNO.pdf). Diseño de Interiores no figura en ninguna fuente |
| Las 10 carreras ocultadas en la primera revisión | Siguen ocultas | Su fuente oficial actual no las ubica en Cochabamba |
| USIP: Administración y Finanzas | Sigue oculta | No figura en el sitio de USIP (ofrece el técnico superior en Administración y Negocios, ya agregado) |

**Se ocultaron 8 carreras que estaban visibles:**

| Institución | Carrera | Estado | Motivo |
|---|---|---|---|
| ULAT | Bioquímica y Farmacia, Ciencias de la Educación, Derecho, Ingeniería de Sistemas, Odontología | `NO CONFIRMADA` | El sitio oficial solo tiene páginas de [Ingeniería Civil, Auditoría, Enfermería y Medicina](https://ulat.edu.bo/wp-sitemap-posts-carrera-1.xml); su página del campus Cochabamba ya no lista carreras |
| INCOS N.° 3 (Quillacollo) | Contaduría General, Secretariado Ejecutivo | `NO CONFIRMADA` | Habían sido estimadas con IA. No hay sitio oficial y la única fuente es un [plan de mejora de 2022](https://ftpnacional.cemse.edu.bo/wp-content/uploads/2022/10/PLAN-DE-MEJORA_IT-INCOS-Na-3.pdf) |
| UMSS | Gastronomía | `SE DICTA FUERA DE LA CIUDAD` | Es un programa desconcentrado de la UMSS que se dicta en Cliza, Valle Alto ([Los Tiempos](https://www.lostiempos.com/taxonomy/term/64926/tema/gastronomia-cliza-64926)) |

**Sin cambios, con sede por confirmar:** UPB (su campus Cochabamba existe, Av. Cap. Víctor Ustáriz km 6.5, pero su web no indica el campus de cada programa; sus 15 carreras del catálogo figuran en su oferta actual) y TECBA (sedes en Cochabamba y Sucre; sus 10 carreras figuran en su menú actual).

**Área afín reemplazada:** «Gestión documental y secretariado» quedó sin ninguna carrera vigente en Cochabamba. Se reemplazó por «Comercio internacional y aduanas» (también del tipo Convencional), que sí tiene carreras (UCB, Univalle y TECBA).

**Estado final en Neon:** 231 carreras visibles (179 verificadas y 52 con sede por confirmar), ninguna estimada con IA y ninguna institución visible fuera de Cochabamba. Las 24 áreas que puede sugerir el test encuentran carreras.
