FROM ubuntu:18.04

# create the app user
RUN addgroup --system app && adduser --system --group app

RUN apt-get update
RUN apt-get install -y wget
RUN wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt-get update
RUN apt-get install -y apt-transport-https
RUN apt-get update
RUN apt-get install -y dotnet-sdk-6.0

# create the appropriate directories
ENV HOME=/home/app
ENV APP_HOME=/home/app/
WORKDIR $APP_HOME

# Copy project from current folder
COPY ./PromotionEngine $APP_HOME

# chown all the files to the app user
RUN chown -R app:app $APP_HOME

# change to the app user which is better than having every container run like root, since attackers can gain root access to docker host if they manage to break out of container.
USER app