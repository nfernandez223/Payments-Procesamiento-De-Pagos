Worker Procesamiento de Pagos

#Instalacion DockerFile en Docker Desktop

1- Tener un ambiente de Docker instalado.

2- Verficar la existencia del archivo Dockerfile dentro del repositorio y su configuracion, siempre debe estar en la raiz del directorio.

3- Abrir una terminal en el directorio donde se encuentra el Dockerfile y ejecuta el siguiente comando para construir la imagen de Docker:
docker build -t procesamientodepagos .

4- Una vez creada la imagen la podremos visualizar dentro del apartado de images en Docker y le damos a RUN.

5- Podemos definir como opcionales variables de entorno, si no lo definimos la imagen va a tomar la configuracion del appsettings.json del servicio.

6- El servicio queda iniciado.


 
