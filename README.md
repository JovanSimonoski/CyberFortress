Оваа веб апликација има за цел да им обезбеди на клиентите безбедно чување на податоци на серверска страна. Имено, секој клиент има свој сеф во кој се чуваат податоците во енкритирана форма. Апликацијта нуди и можност за безбедно споделување на податоци помеѓу клиентите и менаџер на лозинки кој овозможува генерирање и/или чување на лозинките во енкриптирана форма на серверска страна. Акцентот на сите акции и функционалности е ставен на безбедноста и интегритетот на податоците.

*Технички детали

  - Апликацијата е изработена во ASP .NET MVC Web Framework во C#. Истата е хостирана на локален IIS сервер и за целите на апликацијата е креиран и искористен локален CA со цел да се овозможи SSL конекција (HTTPS).

Во продолжение се во детали објаснети функционалностите на апликацијата.

Регистрација на нов корисник, најава на корисник, заборавена лозинка

  -	Со самото креирање на апликацијата е искористен entity framework со individual user accounts кој го нуди .NET за кориснички акции. На апликацијата има можност за регистрирање на нов корисник, најава, промена на лозинка и слично. За сите акции кои може да се преземат на апликацијата е потребно да бидете најавени на истата.

File Safe

  -	Сефот за податоци претставува основната функционалност на оваа веб апликација. Секој корисник има сопствен сеф (директориум на серверот) во кој се чуваат сите негови податоци во енкриптирана форма. 
  -	Корисникот има можност да прикачи одреден file во форма за прикачување на податоци и по испраќањето на формата, file-от ќе биде енкриптиран на серверска страна со помош на AES енкрипција и автоматски изгенериран клуч кој се однесува само на специфичниот file со цел да се зголеми нивото на безбедност. Овој клуч се чува во посебна табела од базата. При прикачување на првиот file од страна на еден корисник, на серверот се креира директориум на специфична локација именуван според ID-то на корисникот и во тој директориум се зачувуваат сите негови податоци/file-ови. При преземање на file-от, истот се декриптира и се испраќа во декриптирана форма на корисничка страна (но секако во енкриптиран пакет).
  -	Пристап до корисничкиот сеф има само корисникот кој е сопственик на истиот, односно само овој корисник има пристап до тајните клучеви кои се користат за енкрипција и декрипција на неговите file-ови.
  -	Корисничкиот сеф има ограничен капацитет со цел да се спречи преоптоварување на серверот и да се обезбеди фер распределба на меморија помеѓу сите корисници.

Shared by me и Shared with me

  -	Оваа функционалнот е релативно слично имплементирана како и сефот за податоци, односно со помош на истата може да прикачиме податоци кои сакаме да ги споделиме со некој од останатите корисници на апликацијата.
  -	Корисникот има можност да прикачи file преку формата за прикачување на податоци и како следен чекор треба да го наведе корисничкото име на корисникот со кој сака да го сподели овој file. На страна на серверот се креира директориум на специфична локација кој е именуван според ID-то на корисникот кој го прикачува file-от и до истиот има пристап и корисникот со кого е споделен овој file. Слично како и податочниот сеф и тука се генерира посебен клуч за секој file и се користи AES енкрипција на серверска страна. При преземање на file-от, истот се декриптира и се испраќа во декриптирана форма на корисничка страна (но секако во енкриптиран пакет).
  -	Пристап до овој сеф имаат само корисникот кој го споделува file-от и корисникот со кого е споделен file-от. И двата корисници имаат можност да ги преземаат и бришат заедничките file-ови.
  -	Овој споделен сеф има ограничен капацитет со цел да се оневозможи неконтролирано искористување на ресурсите на серверот и да се обезбеди фер распределба на меморија помеѓу сите корисници.

Password Manager

  -	Оваа функционалност е додадена како дополнителна секција на страната и истата има за цел да обезбеди безбедно чување на лозинки на серверска страна.
  -	Секој корисник има свој сеф за лозинки во кој може да додава нови лозинки преку соодветна форма. За секоја лозинка е потребно да се внесат веб сајтот за кој е наменета, корисничкото име и самата лозинка. Сајтот нуди можност и за автоматско генерирање на лозинка со однапред избрана големина која нас ни одговара. Откако ќе ја пополниме формата потребно е да внесеме master лозинка со помош на која се врши енкрипција на лозинката со AES на клиентска страна и истата се испраќа и чува во енкриптирана форма на серверска страна со цел никој да нема увид во оригиналната лозинка освен самиот корисник. Во овој дел се прикажани сите зачувани лозинки на корисникот и доколку тој сака да добие некоја од лозинките во оригинална форма, треба да ја внесе master лозинката со која се врши декрипција на лозинката на клиентска страна и истата се испишува во оригиналната форма.
  -	Пристап до сопствениот сеф со лозинки има само корисникот кој е сопственик.
