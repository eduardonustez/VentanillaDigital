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
        "id": 1, "departamento": "Bogot?? D.C.", "ciudades":
        ["Bogot?? D.C.",]
    },
    {
        "id": 2, "departamento": "Amazonas", "ciudades":
            ["Leticia"]
    },
    {
        "id": 3
        , "departamento": "Antioquia", "ciudades":
            ["Abejorral",
            "Amag??",
            "Amalfi",
            "Andes",
            "Angostura",
            "Anor??",
            "Arboletes",
            "Argelia",
            "Armenia",
            "Barbosa",
            "Bello",
            "Betania",
            "Betulia",
            "Brice??o",
            "Buritic??",
            "C??ceres",
            "Caldas",
            "Ca??asgordas",
            "Caramanta",
            "Carmen de Viboral",
            "Carolina",
            "Caucasia",
            "Chigorodo",
            "Cisneros",
            "Ciudad Bol??var",
            "Cocorna",
            "Concordia",
            "Copacabana",
            "Dabeiba",
            "Don Mat??as",
            "Ebejico",
            "El Bagre",
            "El Pe??ol",
            "El Santuario",
            "Entrerrios",
            "Envigado",
            "Fredonia",
            "Frontino",
            "Girardota",
            "Granada",
            "Guarne",
            "Itagui",
            "Jard??n",
            "Jeric??",
            "La Ceja",
            "La Estrella",
            "La Uni??n",
            "Liborina",
            "Maceo",
            "Marinilla",
            "Medell??n",
            "Nari??o",
            "Nech??",
            "Pueblo Rico",
            "Puerto Berrio",
            "Puerto Nare",
            "Puerto Triunfo",
            "Remedios",
            "Retiro",
            "Rionegro",
            "Sabaneta",
            "Salgar",
            "San Andr??s",
            "San Carlos",
            "San Jer??nimo",
            "San Juan de Urab??",
            "San Pedro Belmira",
            "San Rafael",
            "San Roque",
            "San Vicente",
            "Santa Barbara",
            "Santa Rosa de Osos",
            "Santaf?? de Antioquia",
            "Santo Domingo",
            "Segovia",
            "Sons??n",
            "Sopetr??n",
            "T??mesis",
            "Taraz??",
            "Tarso",
            "Titirib??",
            "Urrao",
            "Valdivia",
            "Valparaiso",
            "Vegach??",
            "Venecia",
            "Yal??",
            "Yarumal",
            "Yolomb??",
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
        , "departamento": "Atl??ntico", "ciudades":
            ["Baranoa",
            "Barranquilla",
            "Campo de la Cruz",
            "Galapa",
            "Juan de Acosta",
            "Luruaco",
            "Malambo",
            "Manat??",
            "Ponedera",
            "Repel??n",
            "Sabanalarga",
            "Sabanagrande",
            "Santo T??mas",
            "Soledad",
            ]
    },
    {
        "id": 6
        , "departamento": "Bol??var", "ciudades":
            ["Ach??",
            "Arjona",
            "Barranco de Loba",
            "Calamar",
            "Carmen De Bol??var",
            "Cartagena",
            "C??rdoba",
            "Guamo",
            "Magangu??",
            "Mahates",
            "Mar??a La Baja",
            "Mompos",
            "Morales",
            "Pinillos",
            "Rioviejo",
            "San Estanislao",
            "San Jacinto",
            "San Juan Nepomuceno",
            "San Mart??n De Loba",
            "San Pablo",
            "Santa Catalina",
            "Santa Rosa del Sur",
            "Simit??",
            "Talaigua Nuevo",
            "Turbaco",
            "Zambrano",
            ]
    },
    {
        "id": 7
        , "departamento": "Boyac??", "ciudades":
            ["Aquitania",
            "Bel??n",
            "Boavita",
            "Campohermoso",
            "Chiquinquir??",
            "Chiscas",
            "Chita",
            "Cubar??",
            "Duitama",
            "El Cocuy",
            "Garagoa",
            "Guateque",
            "Guayat??",
            "Labranza Grande",
            "Macanal",
            "Marip??",
            "Miraflores",
            "Moniquir??",
            "Muzo",
            "Nobsa",
            "Paipa",
            "Pauna",
            "Paz De R??o",
            "Pesca",
            "Ramiriqu??",
            "Saboy??",
            "Samac??",
            "San Luis de Gaceno",
            "Santa Rosa De Viterbo",
            "Sativanorte",
            "Soata",
            "Socha",
            "Sogamoso",
            "Sotaquir??",
            "Tenza",
            "Tinjac??",
            "Toca",
            "Tunja",
            "Turmequ??",
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
            "Belalc??zar",
            "Chinchin??",
            "Filadelfia",
            "Manizales",
            "Manzanares",
            "Marmato",
            "Marquetalia",
            "Marulanda",
            "Neira",
            "P??cora",
            "Palestina",
            "Pensilvania",
            "Riosucio",
            "Risaralda",
            "Salamina",
            "Saman??",
            "Sup??a",
            "Victoria",
            "Villa Mar??a",
            "Viterbo",                        
            ]
    },
    {
        "id": 9
        , "departamento": "Caquet??", "ciudades":
            ["Bel??n De Los Andaquies",
            "Cartagena del Chair??",
            "El Doncello",
            "Florencia",
            "La Monta??ita",
            "Puerto Rico",
            "San Vicente Del Cagu??n",                                   
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
            "Bol??var",
            "Buenos Aires",
            "Caloto",
            "El Tambo",
            "Guap??",
            "Inza",
            "La Vega",
            "L??pez de Micay",
            "Mercaderes",
            "Miranda",
            "Morales",
            "Pat??a (El Bordo)",
            "Piendam??",
            "Popay??n",
            "Puerto Tejada",
            "Rosas",
            "San Sebasti??n",
            "Santander De Quilichao",
            "Silvia",
            "Timb??o",
            "Timbiqu??",
            "Torib??o",                                                           
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
            "Curuman??",
            "El Copey",
            "El Paso",
            "Gamarra",
            "Gonz??lez",
            "La Gloria",
            "La Jagua De Ibirico",
            "La Paz (Robles)",
            "Pailitas",
            "Pueblo Bello",
            "R??o De Oro",
            "San Alberto",
            "San Diego",
            "Tamalameque",
            "Valledupar",                                                                       
            ]
    },
    {
        "id": 13
        , "departamento": "Choc??", "ciudades":
            ["Acand??",
            "Alto Baudo (Pie De Pato)",
            "Bah??a Solano",
            "Bajo Baud?? (Pizarro)",
            "Bojaya",
            "Istmina",
            "Jurado",
            "LLor??",
            "Novita",
            "Nuqu??",
            "Quibdo",
            "San Jos?? Del Palmar",
            "S??pi",
            "Tado",
            "Ungu??a",                                                                                  
            ]
    },
    {
        "id": 14
        , "departamento": "C??rdoba", "ciudades":
            ["Ayapel",
            "Buenavista",
            "Ceret??",
            "Chin??",
            "Ci??naga de Oro",
            "Lorica",
            "Montelibano",
            "Monter??a",
            "Planeta Rica",
            "Pueblo Nuevo",
            "Puerto Libertador",
            "Pur??sima",
            "Sahag??n",
            "San Andr??s de Sotavento",
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
            "Bojac??",
            "Caparrapi",
            "Ch??a",
            "Chipaque",
            "Chocont??",
            "Cota",
            "Facatativ??",
            "Fusagasug??",
            "Gachal??",
            "Girardot",
            "Guaduas",
            "Guatavita",
            "Jun??n",
            "La Calera",
            "La Palma",
            "La Vega",
            "Machet??",
            "Madrid",
            "Mosquera",
            "Nemoc??n",
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
            "Ubal??",
            "Ubat??",
            "Une",
            "??tica",
            "Villapinz??n",
            "Villeta",
            "Viot??",
            "Yacop??",
            "Zipaquir??",                                                                                       
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
            ["San Jos?? Del Guaviare",                                                                                  
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
            "Garz??n",
            "Guadalupe",
            "La Plata",
            "Neiva",
            "Palermo",
            "Pitalito",
            "San Agust??n",
            "Tesalia",
            "Timan??",
            "Yaguar??",                                                                                              
            ]
    },
    {
        "id": 19
        , "departamento": "Magdalena", "ciudades":
            ["Algarrobo",
            "Ariguan??",
            "Cerro de San Antonio",
            "Chivolo",
            "El Banco",
            "El Pi??on",
            "Fundaci??n",
            "Guamal",
            "Pedraza",
            "Pivijay",
            "Plato",
            "Remolino",
            "Sabanas de San ??ngel",
            "Salamina",
            "San Sebastian De Buenavista",
            "San Zen??n",
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
            "Puerto L??pez",
            "Puerto Rico",
            "San Juan de Arama",
            "San Mart??n",
            "Uribe",
            "Villavicencio",
            "Vista Hermosa",
                                                                                                                
            ]
    },
    {
        "id": 21
        , "departamento": "Nari??o", "ciudades":
            ["Alb??n",
            "Barbacoas",
            "Buesaco",
            "Cumbal",
            "El Charco",
            "El Tambo",
            "Ipiales",
            "La Cruz",
            "La Uni??n",
            "Linares",
            "Los Andes",
            "Pasto",
            "Puerres",
            "Ricaurte",
            "Samaniego",
            "San Pablo",
            "Sandon??",
            "Taminango",
            "Tumaco",
            "T??querres",                                                                                                   
            ]
    },
    {
        "id": 22
        , "departamento": "Norte de Santander", "ciudades":
            ["Abrego",
            "Arboledas",
            "Bucarasica",
            "C??chira",
            "Chinacota",
            "Convenci??n",
            "C??cuta",
            "Cucutilla",
            "Durania",
            "El Carmen",
            "El Tarra",
            "El Zulia",
            "Gramalote",
            "Hacar??",
            "Los Patios",
            "Oca??a",
            "Pamplona",
            "Puerto Santander",
            "Salazar de las Palmas",
            "San Calixto",
            "Sardinata",
            "Teorama",
            "Tib??",
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
            "Puerto As??s",
            "Puerto Leguizamo",
            "San Miguel",
            "Santiago",
            "Valle Del Guamuez (La Hormiga)",
            "Villa Garz??n",                                                                                                                           
            ]
    },
    {
        "id": 24
        , "departamento": "Quind??o", "ciudades":
            ["Armenia",
            "Calarc??",
            "Circasia",
            "Filandia",
            "G??nova",
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
            ["Ap??a",
            "Balboa",
            "Bel??n de Umbria",
            "Gu??tica",
            "La Celia",
            "La Virginia",
            "Marsella",
            "Mistrat??",
            "Pereira",
            "Pueblo Rico",
            "Quinch??a",                                                                                                                                      
            ]
    },
    {
        "id": 26
        , "departamento": "San Andr??s", "ciudades":
            ["Providencia",
            "San Andr??s Isla",                                                                                                                                                 
            ]
    },
    {
        "id": 27
        , "departamento": "Santander", "ciudades":
            ["Barrancabermeja",
            "Bol??var",
            "Bucaramanga",
            "Charal??",
            "Cimitarra",
            "Concepci??n",
            "Contrataci??n",
            "El Carmen de Chucur??",
            "El Play??n",
            "Gal??n",
            "G??mbita",
            "Gir??n",
            "Guaca",
            "Guadalupe",
            "Jes??s Mar??a",
            "Lebrija",
            "M??laga",
            "Matanza",
            "Mogotes",
            "Oiba",
            "Onzaga",
            "Piedecuesta",
            "Puente Nacional",
            "Puerto Wilches",
            "Rionegro",
            "Sabana De Torres",
            "San Andr??s",
            "San Gil",
            "San Vicente de Chucur??",
            "Simacota",
            "Suaita",
            "V??lez",
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
            "Ricaurte (Colos??)",
            "San Benito Abad",
            "San Marcos",
            "San Onofre",
            "San Pedro",
            "Sinc??",
            "Sincelejo",
            "Sucre",
            "Tol??",
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
            "Ibagu??",
            "L??rida",
            "L??bano",
            "Mariquita",
            "Melgar",
            "Natagaima",
            "Ortega",
            "Planadas",
            "Purificaci??n",
            "Rioblanco",
            "Rovira",
            "Salda??a",
            "San Antonio",
            "San Luis",
            "Venadillo",
            "Villahermosa",                                                                                                                                                                                
            ]
    },
    {
        "id": 30
        , "departamento": "Valle", "ciudades":
            ["Alcal??",
            "Andaluc??a",
            "Ansermanuevo",
            "Argelia",
            "Bol??var",
            "Buenaventura",
            "Buga",
            "Bugalagrande",
            "Caicedonia",
            "Cali",
            "Calima (El Dari??n)",
            "Candelaria",
            "Cartago",
            "Dagua",
            "El ??guila",
            "El Cairo",
            "El Cerrito",
            "El Dovio",
            "Florida",
            "Ginebra",
            "Guacar??",
            "Jamund??",
            "La Cumbre",
            "La Uni??n",
            "La Victoria",
            "Obando",
            "Palmira",
            "Pradera",
            "Restrepo",
            "Riofr??o",
            "Roldanillo",
            "San Pedro",
            "Sevilla",
            "Toro",
            "Trujillo",
            "Tulu??",
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
        , "departamento": "Vaup??s", "ciudades":
            ["Mit??",                                                                                                                                                                                            
            ]
    },
    {
        "id": 32
        , "departamento": "Vichada", "ciudades":
            ["Cumaribo",
            "La Primavera",
            "Puerto Carre??o",
            "Santa Rosal??a",                                                                                                                                                                                                        
            ]
    }] 
}
