# Build docker image to run the application
docker build -t light-manager:1.0 -f .\Dockerfile .

# Publish docker image to custom registry
docker tag light-manager:1:0 192.168.1.100:5000/light-manager:1.0