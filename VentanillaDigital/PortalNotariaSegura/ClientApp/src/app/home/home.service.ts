import { Inject, Injectable, OnInit } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, delay, map, shareReplay } from 'rxjs/operators';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Notaria } from '../models/notaria.model';

const CACHE_SIZE = 1;
@Injectable({
    providedIn: 'root'
})
export class HomeService implements OnInit {

    private isCaptchaValidated: boolean = false;
    private notariasCache$: Observable<Notaria[]>;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    ngOnInit() {
        this.NotariasArray;
    }

    get NotariasArray() {
        if (!this.notariasCache$) {
            this.notariasCache$ = this.GetDataNotariasFromApi()
                .pipe(
                    shareReplay(CACHE_SIZE)
                );
        }
        return this.notariasCache$;
    }

    getDepartmentsAndCities() {
        let items = getMockCities();
        return of(items)
    }
    
    setResultCaptcha(result: string) {
        this.isCaptchaValidated = false;
        if (result != null && result != undefined && result != '') {
            this.isCaptchaValidated = true;
        }
    }
    getResultCaptcha() {
        return this.isCaptchaValidated;
    }

    getActaNotarial(notariaId: string, fechaTramite: string, email: string, tramiteId: string) {
    //debugger;
    return this.http.get(this.baseUrl + 'documents/getactanotarial/'+notariaId+'/'+fechaTramite+'/'+email+'/'+tramiteId, {responseType: 'text'}).pipe(
        map(response => {
            let settings = { 
                "data": response, 
                "tramiteId": tramiteId
            }            
             
            return settings;
            }
            ),
            catchError(this.handleError)
        );
    }

    handleError(err: HttpErrorResponse) {
        let errorMessage = '';
        if (err.error instanceof ErrorEvent) {

            errorMessage = `An error occurred: ${err.error.message}`;
        } else {

            errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
        }
        console.error(errorMessage);
        return throwError(errorMessage);
    }

    private GetDataNotariasFromApi() {
        return this.http.get<Notaria[]>(this.baseUrl + 'documents/obtenernotarias')
            .pipe(
                map(response => response)
            );
    }
}

function getMockCities() {
    return [
    {
        "id": 1, "departamento": "Bogotá D.C.", "ciudades":
        ["Bogotá D.C.",]
    },
    {
        "id": 2, "departamento": "Amazonas", "ciudades":
            ["Leticia"]
    },
    {
        "id": 3
        , "departamento": "Antioquia", "ciudades":
            ["Abejorral",
            "Amagá",
            "Amalfi",
            "Andes",
            "Angostura",
            "Anorí",
            "Arboletes",
            "Argelia",
            "Armenia",
            "Barbosa",
            "Bello",
            "Betania",
            "Betulia",
            "Briceño",
            "Buriticá",
            "Cáceres",
            "Caldas",
            "Cañasgordas",
            "Caramanta",
            "Carmen de Viboral",
            "Carolina",
            "Caucasia",
            "Chigorodo",
            "Cisneros",
            "Ciudad Bolívar",
            "Cocorna",
            "Concordia",
            "Copacabana",
            "Dabeiba",
            "Don Matías",
            "Ebejico",
            "El Bagre",
            "El Peñol",
            "El Santuario",
            "Entrerrios",
            "Envigado",
            "Fredonia",
            "Frontino",
            "Girardota",
            "Granada",
            "Guarne",
            "Itagui",
            "Jardín",
            "Jericó",
            "La Ceja",
            "La Estrella",
            "La Unión",
            "Liborina",
            "Maceo",
            "Marinilla",
            "Medellín",
            "Nariño",
            "Nechí",
            "Pueblo Rico",
            "Puerto Berrio",
            "Puerto Nare",
            "Puerto Triunfo",
            "Remedios",
            "Retiro",
            "Rionegro",
            "Sabaneta",
            "Salgar",
            "San Andrés",
            "San Carlos",
            "San Jerónimo",
            "San Juan de Urabá",
            "San Pedro Belmira",
            "San Rafael",
            "San Roque",
            "San Vicente",
            "Santa Barbara",
            "Santa Rosa de Osos",
            "Santafé de Antioquia",
            "Santo Domingo",
            "Segovia",
            "Sonsón",
            "Sopetrán",
            "Támesis",
            "Tarazá",
            "Tarso",
            "Titiribí",
            "Urrao",
            "Valdivia",
            "Valparaiso",
            "Vegachí",
            "Venecia",
            "Yalí",
            "Yarumal",
            "Yolombó",
            "Zaragoza",
            ]
    },  
    {
        "id": 4
        , "departamento": "Arauca", "ciudades":
            ["Arauca",
            "Arauquita",
            "Cravo Norte",
            "Fortul",
            "Puerto Rondon",
            "Saravena",
            "Tame",    
            ]
    },
    {
        "id": 5
        , "departamento": "Atlántico", "ciudades":
            ["Baranoa",
            "Barranquilla",
            "Campo de la Cruz",
            "Galapa",
            "Juan de Acosta",
            "Luruaco",
            "Malambo",
            "Manatí",
            "Ponedera",
            "Repelón",
            "Sabanalarga",
            "Sabanagrande",
            "Santo Tómas",
            "Soledad",
            ]
    },
    {
        "id": 6
        , "departamento": "Bolívar", "ciudades":
            ["Achí",
            "Arjona",
            "Barranco de Loba",
            "Calamar",
            "Carmen De Bolívar",
            "Cartagena",
            "Córdoba",
            "Guamo",
            "Magangué",
            "Mahates",
            "María La Baja",
            "Mompos",
            "Morales",
            "Pinillos",
            "Rioviejo",
            "San Estanislao",
            "San Jacinto",
            "San Juan Nepomuceno",
            "San Martín De Loba",
            "San Pablo",
            "Santa Catalina",
            "Santa Rosa del Sur",
            "Simití",
            "Talaigua Nuevo",
            "Turbaco",
            "Zambrano",
            ]
    },
    {
        "id": 7
        , "departamento": "Boyacá", "ciudades":
            ["Aquitania",
            "Belén",
            "Boavita",
            "Campohermoso",
            "Chiquinquirá",
            "Chiscas",
            "Chita",
            "Cubará",
            "Duitama",
            "El Cocuy",
            "Garagoa",
            "Guateque",
            "Guayatá",
            "Labranza Grande",
            "Macanal",
            "Maripí",
            "Miraflores",
            "Moniquirá",
            "Muzo",
            "Nobsa",
            "Paipa",
            "Pauna",
            "Paz De Río",
            "Pesca",
            "Ramiriquí",
            "Saboyá",
            "Samacá",
            "San Luis de Gaceno",
            "Santa Rosa De Viterbo",
            "Sativanorte",
            "Soata",
            "Socha",
            "Sogamoso",
            "Sotaquirá",
            "Tenza",
            "Tinjacá",
            "Toca",
            "Tunja",
            "Turmequé",
            "Umbita",
            "Ventaquemada",
            "Zetaquira",            
            ]
    },
    {
        "id": 8
        , "departamento": "Caldas", "ciudades":
            ["Aguadas",
            "Anserma",
            "Aranzazu",
            "Belalcázar",
            "Chinchiná",
            "Filadelfia",
            "Manizales",
            "Manzanares",
            "Marmato",
            "Marquetalia",
            "Marulanda",
            "Neira",
            "Pácora",
            "Palestina",
            "Pensilvania",
            "Riosucio",
            "Risaralda",
            "Salamina",
            "Samaná",
            "Supía",
            "Victoria",
            "Villa María",
            "Viterbo",                        
            ]
    },
    {
        "id": 9
        , "departamento": "Caquetá", "ciudades":
            ["Belén De Los Andaquies",
            "Cartagena del Chairá",
            "El Doncello",
            "Florencia",
            "La Montañita",
            "Puerto Rico",
            "San Vicente Del Caguán",                                   
            ]
    },
    {
        "id": 10
        , "departamento": "Casanare", "ciudades":
            ["Hato Corozal",
            "Monterrey",
            "Nunchia",
            "Paz De Ariporo",
            "Tauramena",
            "Villanueva",
            "Yopal",                                               
            ]
    },
    {
        "id": 11
        , "departamento": "Cauca", "ciudades":
            ["Almaguer",
            "Balboa",
            "Bolívar",
            "Buenos Aires",
            "Caloto",
            "El Tambo",
            "Guapí",
            "Inza",
            "La Vega",
            "López de Micay",
            "Mercaderes",
            "Miranda",
            "Morales",
            "Patía (El Bordo)",
            "Piendamó",
            "Popayán",
            "Puerto Tejada",
            "Rosas",
            "San Sebastián",
            "Santander De Quilichao",
            "Silvia",
            "Timbío",
            "Timbiquí",
            "Toribío",                                                           
            ]
    },
    {
        "id": 12
        , "departamento": "Cesar", "ciudades":
            ["Aguachica",
            "Agustin Codazzi",
            "Astrea",
            "Bosconia",
            "Chimichagua",
            "Chiriguana",
            "Curumaní",
            "El Copey",
            "El Paso",
            "Gamarra",
            "González",
            "La Gloria",
            "La Jagua De Ibirico",
            "La Paz (Robles)",
            "Pailitas",
            "Pueblo Bello",
            "Río De Oro",
            "San Alberto",
            "San Diego",
            "Tamalameque",
            "Valledupar",                                                                       
            ]
    },
    {
        "id": 13
        , "departamento": "Chocó", "ciudades":
            ["Acandí",
            "Alto Baudo (Pie De Pato)",
            "Bahía Solano",
            "Bajo Baudó (Pizarro)",
            "Bojaya",
            "Istmina",
            "Jurado",
            "LLoró",
            "Novita",
            "Nuquí",
            "Quibdo",
            "San José Del Palmar",
            "Sípi",
            "Tado",
            "Unguía",                                                                                  
            ]
    },
    {
        "id": 14
        , "departamento": "Córdoba", "ciudades":
            ["Ayapel",
            "Buenavista",
            "Cereté",
            "Chinú",
            "Ciénaga de Oro",
            "Lorica",
            "Montelibano",
            "Montería",
            "Planeta Rica",
            "Pueblo Nuevo",
            "Puerto Libertador",
            "Purísima",
            "Sahagún",
            "San Andrés de Sotavento",
            "San Antero",
            "San Bernardo Del Viento",
            "San Carlos",
            "San Jose de Ure",
            "San Pelayo",
            "Tierralta",
            "Valencia",                                                                                         
            ]
    },
    {
        "id": 15
        , "departamento": "Cundinamarca", "ciudades":
            ["Agua de Dios",
            "Anapoima",
            "Anolaima",
            "Bojacá",
            "Caparrapi",
            "Chía",
            "Chipaque",
            "Chocontá",
            "Cota",
            "Facatativá",
            "Fusagasugá",
            "Gachalá",
            "Girardot",
            "Guaduas",
            "Guatavita",
            "Junín",
            "La Calera",
            "La Palma",
            "La Vega",
            "Machetá",
            "Madrid",
            "Mosquera",
            "Nemocón",
            "Nocaima",
            "Pacho",
            "Paime",
            "Puerto Salgar",
            "San Francisco",
            "San Juan de Rioseco",
            "Sasaima",
            "Sesquile",
            "Silvania",
            "Simijaca",
            "Soacha",
            "Subachoque",
            "Tabio",
            "Tocaima",
            "Ubalá",
            "Ubaté",
            "Une",
            "Útica",
            "Villapinzón",
            "Villeta",
            "Viotá",
            "Yacopí",
            "Zipaquirá",                                                                                       
            ]
    },
    {
        "id": 16
        , "departamento": "Guajira", "ciudades":
            ["Barrancas",
            "Maicao",
            "Riohacha",
            "San Juan del Cesar",
            "Uribia",
            "Villanueva",                                                                                                
            ]
    },
    {
        "id": 17
        , "departamento": "Guaviare", "ciudades":
            ["San José Del Guaviare",                                                                                  
            ]
    },
    {
        "id": 18
        , "departamento": "Huila", "ciudades":
            ["Acevedo",
            "Agrado",
            "Algeciras",
            "Baraya",
            "Campoalegre",
            "Colombia",
            "Garzón",
            "Guadalupe",
            "La Plata",
            "Neiva",
            "Palermo",
            "Pitalito",
            "San Agustín",
            "Tesalia",
            "Timaná",
            "Yaguará",                                                                                              
            ]
    },
    {
        "id": 19
        , "departamento": "Magdalena", "ciudades":
            ["Algarrobo",
            "Ariguaní",
            "Cerro de San Antonio",
            "Chivolo",
            "El Banco",
            "El Piñon",
            "Fundación",
            "Guamal",
            "Pedraza",
            "Pivijay",
            "Plato",
            "Remolino",
            "Sabanas de San Ángel",
            "Salamina",
            "San Sebastian De Buenavista",
            "San Zenón",
            "Santa Ana",
            "Santa Marta",
            "Sitio Nuevo",
            "Tenerife",
            "Zona Bananera",                                                                                               
            ]
    },
    {
        "id": 20
        , "departamento": "Meta", "ciudades":
            ["Granada",
            "La Macarena",
            "Puerto Gaitan",
            "Puerto López",
            "Puerto Rico",
            "San Juan de Arama",
            "San Martín",
            "Uribe",
            "Villavicencio",
            "Vista Hermosa",
                                                                                                                
            ]
    },
    {
        "id": 21
        , "departamento": "Nariño", "ciudades":
            ["Albán",
            "Barbacoas",
            "Buesaco",
            "Cumbal",
            "El Charco",
            "El Tambo",
            "Ipiales",
            "La Cruz",
            "La Unión",
            "Linares",
            "Los Andes",
            "Pasto",
            "Puerres",
            "Ricaurte",
            "Samaniego",
            "San Pablo",
            "Sandoná",
            "Taminango",
            "Tumaco",
            "Túquerres",                                                                                                   
            ]
    },
    {
        "id": 22
        , "departamento": "Norte de Santander", "ciudades":
            ["Abrego",
            "Arboledas",
            "Bucarasica",
            "Cáchira",
            "Chinacota",
            "Convención",
            "Cúcuta",
            "Cucutilla",
            "Durania",
            "El Carmen",
            "El Tarra",
            "El Zulia",
            "Gramalote",
            "Hacarí",
            "Los Patios",
            "Ocaña",
            "Pamplona",
            "Puerto Santander",
            "Salazar de las Palmas",
            "San Calixto",
            "Sardinata",
            "Teorama",
            "Tibú",
            "Toledo",
            "VILLA CARO",
            "Villa del Rosario",                                                                                                               
            ]
    },
    {
        "id": 23
        , "departamento": "Putumayo", "ciudades":
            ["Mocoa",
            "Orito",
            "Puerto Asís",
            "Puerto Leguizamo",
            "San Miguel",
            "Santiago",
            "Valle Del Guamuez (La Hormiga)",
            "Villa Garzón",                                                                                                                           
            ]
    },
    {
        "id": 24
        , "departamento": "Quindío", "ciudades":
            ["Armenia",
            "Calarcá",
            "Circasia",
            "Filandia",
            "Génova",
            "La Tebaida",
            "Montenegro",
            "Pijao",
            "Quimbaya",
            "Salento",                                                                                                                                      
            ]
    },
    {
        "id": 25
        , "departamento": "Risaralda", "ciudades":
            ["Apía",
            "Balboa",
            "Belén de Umbria",
            "Guática",
            "La Celia",
            "La Virginia",
            "Marsella",
            "Mistrató",
            "Pereira",
            "Pueblo Rico",
            "Quinchía",                                                                                                                                      
            ]
    },
    {
        "id": 26
        , "departamento": "San Andrés", "ciudades":
            ["Providencia",
            "San Andrés Isla",                                                                                                                                                 
            ]
    },
    {
        "id": 27
        , "departamento": "Santander", "ciudades":
            ["Barrancabermeja",
            "Bolívar",
            "Bucaramanga",
            "Charalá",
            "Cimitarra",
            "Concepción",
            "Contratación",
            "El Carmen de Chucurí",
            "El Playón",
            "Galán",
            "Gámbita",
            "Girón",
            "Guaca",
            "Guadalupe",
            "Jesús María",
            "Lebrija",
            "Málaga",
            "Matanza",
            "Mogotes",
            "Oiba",
            "Onzaga",
            "Piedecuesta",
            "Puente Nacional",
            "Puerto Wilches",
            "Rionegro",
            "Sabana De Torres",
            "San Andrés",
            "San Gil",
            "San Vicente de Chucurí",
            "Simacota",
            "Suaita",
            "Vélez",
            "Zapatoca",                                                                                                                                                            
            ]
    },
    {
        "id": 28
        , "departamento": "Sucre", "ciudades":
            ["Caimito",
            "Corozal",
            "Galeras",
            "Guaranda",
            "Los Palmitos",
            "Majagual",
            "Ovejas",
            "Palmito",
            "Ricaurte (Colosó)",
            "San Benito Abad",
            "San Marcos",
            "San Onofre",
            "San Pedro",
            "Sincé",
            "Sincelejo",
            "Sucre",
            "Tolú",
            "Toluviejo",                                                                                                                                                                     
            ]
    },
    {
        "id": 29
        , "departamento": "Tolima", "ciudades":
            ["Ambalema",
            "Armero-Guayabal",
            "Ataco",
            "Cajamarca",
            "Chaparral",
            "Coyaima",
            "Cunday",
            "Dolores",
            "Espinal",
            "Flandes",
            "Fresno",
            "Guamo",
            "Herveo",
            "Ibagué",
            "Lérida",
            "Líbano",
            "Mariquita",
            "Melgar",
            "Natagaima",
            "Ortega",
            "Planadas",
            "Purificación",
            "Rioblanco",
            "Rovira",
            "Saldaña",
            "San Antonio",
            "San Luis",
            "Venadillo",
            "Villahermosa",                                                                                                                                                                                
            ]
    },
    {
        "id": 30
        , "departamento": "Valle", "ciudades":
            ["Alcalá",
            "Andalucía",
            "Ansermanuevo",
            "Argelia",
            "Bolívar",
            "Buenaventura",
            "Buga",
            "Bugalagrande",
            "Caicedonia",
            "Cali",
            "Calima (El Darién)",
            "Candelaria",
            "Cartago",
            "Dagua",
            "El Águila",
            "El Cairo",
            "El Cerrito",
            "El Dovio",
            "Florida",
            "Ginebra",
            "Guacarí",
            "Jamundí",
            "La Cumbre",
            "La Unión",
            "La Victoria",
            "Obando",
            "Palmira",
            "Pradera",
            "Restrepo",
            "Riofrío",
            "Roldanillo",
            "San Pedro",
            "Sevilla",
            "Toro",
            "Trujillo",
            "Tuluá",
            "Ulloa",
            "Versalles",
            "Vijes",
            "Yotoco",
            "Yumbo",
            "Zarzal",                                                                                                                                                                                            
            ]
    },
    {
        "id": 31
        , "departamento": "Vaupés", "ciudades":
            ["Mitú",                                                                                                                                                                                            
            ]
    },
    {
        "id": 32
        , "departamento": "Vichada", "ciudades":
            ["Cumaribo",
            "La Primavera",
            "Puerto Carreño",
            "Santa Rosalía",                                                                                                                                                                                                        
            ]
    }] 
}
