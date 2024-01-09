import flask
import codebert_embeddings
import singberta_embeddings
import pandas as pd
from single_file_pipeline import calculate_distances
from single_file_pipeline import check_vulnerability

app = flask.Flask(__name__)

# accept a JSON string as input with two fields: code and model
@app.route('/predict', methods=['POST'])
def predict():
    try:
        # read in JSON input
        input_json = flask.request.json

        # get code and model from JSON
        code = input_json['code']
        model = input_json['model']
        vulnerability = input_json['vulnerability']
    except:
        return flask.jsonify({'result': 'Error: error reading input, maybe the JSON was not well formatted. Try URL encoding the code', 
                            'references': []})
    try:
        # do something with code and model
        if model == 'codebert' or model == 'singberta':
            snippet_embeddings = {"ME": codebert_embeddings.extract_embeddings_codebert_from_string(code)}

            dfAnalyzedEmbeddings = pd.DataFrame.from_dict(snippet_embeddings, orient='index')

            if vulnerability == 'input_validation': 
                # now read the embeddings of the reference files from database/embeddings_codebert_input_validation.csv
                dfRefEmbeddings = pd.read_csv(f'database/embeddings_{model}_input_validation.csv', header=None, sep='$', index_col=0) 
            elif vulnerability == 'sql_injection':
                # now read the embeddings of the reference files from database/embeddings_codebert_sql_injection.csv
                dfRefEmbeddings = pd.read_csv(f'database/embeddings_{model}_sql_injection.csv', header=None, sep='$', index_col=0)
            else:
                vulnerability_result = 'Error: vulnerability not supported'
                return flask.jsonify({'result': vulnerability_result, 
                                    'references': []})    

            # change the reference embeddings to a dictionary
            dictReferenceEmbeddings = dfRefEmbeddings.to_dict('index')

            # calculate the distances between the reference and analyzed code
            dfDistances = calculate_distances(dictReferenceEmbeddings, snippet_embeddings)

            # check vulnerability
            vulnerability_result, lstReferences = check_vulnerability(dfDistances)

            # return a JSON string
            return flask.jsonify({'result': vulnerability_result, 
                                'references': lstReferences})
        else:
            return flask.jsonify({'result': 'Error: model not supported', 
                                'references': []})
    except:
        return flask.jsonify({'result': 'Error: problem with processing the code or model not available', 
                            'references': []})

# define an endpoint that accepts the same json as above and return the code that was sent to it
@app.route('/echo', methods=['POST'])
def echo():
    # read in JSON input
    input_json = flask.request.json

    # get code and model from JSON
    code = input_json['code']
    model = input_json['model']
    vulnerability = input_json['vulnerability']

    # return a JSON string
    return flask.jsonify({'code': code, 'model': model, 'vulnerability': vulnerability})

# define the endpoint which shows how many vulnerabilities are in the csv file
@app.route('/vulnerabilities', methods=['GET'])
def vulnerabilities():
    # read in the csv file
    df = pd.read_csv('database/embeddings_codebert_input_validation.csv', header=None, sep='$', index_col=0)

    # return a JSON string
    return flask.jsonify({'vulnerabilities': df.shape[0]})

# define the standard endpoint at / that displays a webpage from the web foldes
@app.route('/')
def index():
    # the template is in the web folder
    return flask.render_template('index.html')


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001, debug=True)
