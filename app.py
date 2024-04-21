from flask import Flask

app = Flask(__name__)

@app.route('/hello')
def hello():
    return 'Hello! from k8s'

@app.route('/world')
def world():
    return 'World! from k8s'

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
