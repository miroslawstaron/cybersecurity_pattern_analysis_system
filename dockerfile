FROM pytorch/pytorch:latest
RUN apt update && apt install -y git
WORKDIR /app
COPY . /app
RUN rm -rf /app/src/pyccflex && pip --no-cache-dir install -q -r requirements.txt
CMD ["python3", "app.py"]