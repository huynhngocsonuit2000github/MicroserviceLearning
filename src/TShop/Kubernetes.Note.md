# Generare tls.key and ca.csr file
openssl req -nodes -newkey rsa:2048 -keyout tls.key  -out ca.csr -subj "//CN=xuanthulab.net"
openssl x509 -req -sha256 -days 365 -in ca.csr -signkey tls.key -out tls.crt

# Create secure config within kubernete environment
kubectl create secret tls secret-nginx-cert --cert=tls.crt  --key=tls.key

# Check secret
kubectl describe secret/secret-nginx-cert




# Generate a private key
openssl genrsa -out tls.key 2048

# Generate a certificate signing request (CSR)
openssl req -new -key tls.key -out tls.csr

# Generate a self-signed certificate (valid for 365 days)
openssl x509 -req -days 365 -in tls.csr -signkey tls.key -out tls.crt
