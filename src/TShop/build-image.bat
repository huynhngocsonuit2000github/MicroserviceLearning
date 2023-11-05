:: Build the Docker images
docker build -t tshop-catalog-img -f .\Services\Catalog\Catalog.Api\Dockerfile .
docker build -t tshop-authenticate-img -f .\Services\Authenticate\Authenticate.Api\Dockerfile .
docker build -t tshop-users-img -f .\Services\User\User.Api\Dockerfile .
docker build -t tshop-discounts-img -f .\Services\Discount\Discount.Api\Dockerfile .
docker build -t tshop-basket-img -f .\Services\Basket\Basket.Api\Dockerfile .
docker build -t tshop-ordering-img -f .\Services\Ordering\Ordering.Api\Dockerfile .
docker build -t tshop-apigateway-img -f .\Services\Gateway\ApiGateway\Dockerfile .
