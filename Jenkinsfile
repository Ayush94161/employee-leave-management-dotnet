pipeline {
    agent any

    environment {
        IMAGE_NAME = 'leave-management-api'
    }

    stages {
        stage('Build and Test') {
            steps {
                bat 'docker build --target build -t %IMAGE_NAME%:build .'
            }
        }

        stage('Build Docker Image') {
            steps {
                bat 'docker build -t %IMAGE_NAME%:latest .'
            }
        }

        stage('Deploy') {
            steps {
                bat 'docker compose down'
                bat 'docker compose up -d --build'
            }
        }

        stage('Health Check') {
            steps {
               bat 'curl --retry 12 --retry-delay 5 --retry-all-errors -f http://localhost:5000/health'
            }
        }
    }

    post {
        always {
            bat 'docker image prune -f'
        }
    }
}