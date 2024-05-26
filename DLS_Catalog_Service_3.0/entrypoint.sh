#!/bin/bash

# Wait for MongoDB to be ready
/wait-for-it.sh $MONGO_HOST:27017 --timeout=60 --strict -- echo "MongoDB is up"

# Wait for RabbitMQ to be ready
/wait-for-it.sh $RABBITMQ_HOST:5672 --timeout=60 --strict -- echo "RabbitMQ is up"

# Start the application
dotnet DLS_Catalog_Service_3.0.dll
