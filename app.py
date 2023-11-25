import flask
import codebert_embeddings
import singberta_embeddings

app = flask.Flask(__name__)

# accept a JSON string as input with two fields: code and model
@app.route('/predict', methods=['POST'])
def predict():
    # read in JSON input
    input_json = flask.request.json

    # get code and model from JSON
    code = input_json['code']
    model = input_json['model']

    # do something with code and model
    if model == 'codebert':
        snippet_embeddings = codebert_embeddings.extract_embeddings_codebert_from_string(code)

        result = snippet_embeddings.tolist()
    elif model == 'singberta':
        snippet_embeddings = singberta_embeddings.extract_embeddings_singberta_from_string(code)

        result = snippet_embeddings.tolist()

    # return a JSON string
    return flask.jsonify({'result': result})

# define the standard endpoint at / that displays a webpage from the web foldes
@app.route('/')
def index():
    # the template is in the web folder
    return flask.render_template('index.html')


if __name__ == '__main__':
    app.run(debug=True)
