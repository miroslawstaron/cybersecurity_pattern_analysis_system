FROM ubuntu:latest
RUN apt update && apt install python3 python3-pip -y git
WORKDIR /app
COPY . /app
RUN rm -rf /app/src/pyccflex
RUN pip --no-cache-dir install -q -r requirements.txt
CMD ["python3", "app.py"]