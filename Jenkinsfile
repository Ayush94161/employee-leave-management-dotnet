pipeline {
    agent any
    environment {
        IMAGE_NAME = 'leave-management-api'
        CONTAINER_NAME = 'leave-management-api'
    }
    stages {
        stage('Checkout') { steps { checkout scm } }
        stage('Build and Test') {
            steps { sh 'docker build --target build -t ${IMAGE_NAME}:build .' }
        }
        stage('Build Runtime Image') {
            steps { sh 'docker build -t ${IMAGE_NAME}:${BUILD_NUMBER} -t ${IMAGE_NAME}:latest .' }
        }
        stage('Deploy') {
            steps {
                sh 'docker compose down || true'
                sh 'docker compose up -d --build'
            }
        }
        stage('Health Check') {
            steps {
                sh 'for i in 1 2 3 4 5 6; do curl -fsS http://localhost:8080/health && exit 0; sleep 5; done; exit 1'
            }
        }
    }
    post {
        always { sh 'docker image prune -f || true' }
        success { echo 'CI/CD pipeline completed successfully.' }
        failure { sh 'docker compose logs --no-color || true' }
    }
}
