FROM jhonatans01/python-dlib-opencv
COPY . /app
WORKDIR /app
RUN pip install -r /app/requirements.txt
RUN chmod +x app.py
#ENTRYPOINT ["tail", "-f", "/dev/null"]
ENTRYPOINT ["python3", "app.py"]