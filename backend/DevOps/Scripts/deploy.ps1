Write-Host ""
Write-Host "KYVARA Enterprise Deploy"

docker compose -f ..\Docker\docker-compose.yml up -d

kubectl apply -f ..\Kubernetes\deployment.yaml

kubectl apply -f ..\Kubernetes\service.yaml
