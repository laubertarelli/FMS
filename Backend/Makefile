up: ## Run the project
	@echo "Running the container"
	docker compose up -d --build

down: ## Stop the project
	@echo "Stopping the container"
	docker compose down

down-clear: ## Stop the project and clear the data
	@echo "Stopping the container and clearing the data"
	docker compose down -v

restart: down up ## Restart the project

publish: ## Publish the project
	@echo "Publishing the container"
	docker build -t ghcr.io/laubertarelli/fms-api:latest . --no-cache
	docker push ghcr.io/laubertarelli/fms-api:latest