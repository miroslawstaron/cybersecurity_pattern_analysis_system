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
        return flask.jsonify({'result': 'Error: error processing input', 
                            'references': []})

# define the standard endpoint at / that displays a webpage from the web foldes
@app.route('/')
def index():
    # the template is in the web folder
    return flask.render_template('index.html')


if __name__ == '__main__':
    app.run(debug=True)
