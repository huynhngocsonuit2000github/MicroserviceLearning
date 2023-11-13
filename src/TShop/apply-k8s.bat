:: Catalog
kubectl create configmap app-config --from-file=config.yaml
kubectl apply -f catalog.database.yaml
kubectl apply -f catalog.service.yaml