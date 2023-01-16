openssl req -x509 -newkey rsa:4096 -keyout notes.key -out notes.crt -sha256 -days 365 -subj '/CN=localhost'
    