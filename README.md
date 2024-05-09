# BirthdayGreetings
TDD Kata

## Felicitaciones de cumpleaños
Como eres una persona muy amigable, te gustaría enviar una nota de cumpleaños a todos los amigos que tienes. Pero tienes muchos amigos y eres un poco vago, puede que te lleve algunas veces escribir todas las notas a mano.

La buena noticia es que este sistema puede hacerlo automáticamente por tí.

Imagina que tienes un archivo plano con todos tus amigos:
```
last_name, first_name, date_of_birth, email
 Doe, John, 1982/10/08, john.doe@foobar.com
 Ann, Mary, 1975/09/11, mary.ann@foobar.com
```

Y desea enviarles un correo de feliz cumpleaños en su fecha de nacimiento:

```
Subject: Happy birthday!
Happy birthday, dear <first_name>!
```

¿Cómo sería este software? Intente implementarlo para que pueda cambiar fácilmente:

- la forma en que recupera los datos de sus amigos (por ejemplo, intente cambiar a SQLite Db)
- la forma en que envías la nota: (por ejemplo, imagina que quieres enviar SMS en lugar de correos electrónicos)

¿Qué tipo de pruebas escribirías? ¿Usarías mocks?

## Características adicionales
- Los amigos nacidos el 29 de febrero deberían celebrar su cumpleaños el 28 de febrero.

- Envía una nota de recordatorio de cumpleaños cuando sea el cumpleaños de otra persona:
```
Subject: Birthday Reminder

Dear <first_name>,
Today is <someone_else_first_name> <someone_else_last_name>'s birthday.
Don't forget to send him a message !
``` 
    
- Enviar una única nota de recordatorio de cumpleaños para todas las personas que lo estén hoy.
```
Subject: Birthday Reminder

Dear <first_name>,
Today is <full_name_1>, <full_name_2> and <full_name_3>'s birthday.
Don't forget to send them a message !
``` 
    
