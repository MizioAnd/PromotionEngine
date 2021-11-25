docker image build -f PromotionEngine/DockerDeploy/Dockerfile -t promotion_engine_base:v1.1 .
docker run --name promotion_engine_test -ti promotion_engine_base:v1.1