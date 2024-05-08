import flask
import codebert_embeddings
import singberta_embeddings
import pandas as pd
from single_file_pipeline import calculate_distances
from single_file_pipeline import calculate_distances_from_dict
from single_file_pipeline import check_vulnerability
from single_file_pipeline import predict_large
from single_file_pipeline import predict_small
from werkzeug.utils import secure_filename
import os
import random

# chunk size for large code
chunk_size = 50

app = flask.Flask(__name__)

# accept a JSON string as input with two fields: code and model
@app.route('/predict', methods=['POST'])
def predict():
    steps = []
    dictResult = {}

    steps.append('starting predict endpoint')

    try:
        # Check if a file was uploaded
        if 'file' in flask.request.files:
            # Get the uploaded file from the request
            file = flask.request.files['file']

            # Secure the filename to avoid directory traversal attacks
            filename = secure_filename(file.filename)

            # Save the uploaded file to a temporary directory
            file.save(os.path.join('workdir/temp', filename))

            # Open the saved file and read its contents
            with open(os.path.join('workdir/temp', filename), 'r') as f:
                code = f.readlines()

            # remove the saved file
            os.remove(os.path.join('workdir/temp', filename))

            # Get the 'model' form parameter from the request
            model = flask.request.form.get('model')

            # Get the 'vulnerability' form parameter from the request
            vulnerability = flask.request.form.get('vulnerability')
        else:
            # this part is when we only ask for a small code, part of the 
            # actual JSON input
            # read in JSON input 
            input_json = flask.request.json

            # get code and model from JSON
            code = input_json['code']
            model = input_json['model']
            vulnerability = input_json['vulnerability']

    except Exception as e:
        return flask.jsonify({'result': 'Error: error reading input, maybe the JSON was not well formatted. Try URL encoding the code', 
                            'references': [],
                            'Error': e.message})
    
    steps.append('successfully read input JSON')

    # if the number of lines in the code is larger than 100, then use the predict_large method
    try:
        lenCode = len(code)
        if lenCode > 2 * chunk_size:
            dictResult = predict_large(code, model, vulnerability, steps, chunk_size)
            return flask.jsonify(dictResult)
        else: 
            dictResult[0] = predict_small(code, model, vulnerability, steps)
            return flask.jsonify(dictResult)
    except Exception as e:
        return flask.jsonify({'result': 'Error: problem with processing the code or model not available', 
                              'Error': str(e),
                              'references': steps})


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
    df = pd.read_csv('database/embeddings_codebert.csv', sep='$')

    dictVulnerabilities = df.groupby('vulnerability').count()['filename'].to_dict()

    df = pd.read_csv('database/embeddings_singberta.csv', sep='$')

    dictVulnerabilities2 = df.groupby('vulnerability').count()['filename'].to_dict()

    # merge these two dictionaries
    dictVulnerabilitiesAll = {'CodeBert': dictVulnerabilities, 'SingBerta': dictVulnerabilities2}

    # df.groupby('vulnerability').count()['filename'].to_json()
    # return a JSON string
    return flask.jsonify(dictVulnerabilitiesAll)
    

# define endpoint to add new example to the database
# which accepts a JSON string with the following fields: code, model, vulnerability, type (positive or negative)
@app.route('/add_example', methods=['POST'])
def add_example():
    steps = []
    try:
        # read in JSON input
        input_json = flask.request.json

        # get code and model from JSON
        code = input_json['code']
        model = input_json['model']
        vulnerability = input_json['vulnerability']
        type = input_json['type']
    except:
        return flask.jsonify({'result': 'Error: error reading input, maybe the JSON was not well formatted. Try URL encoding the code', 
                            'references': []})
    try:
        # extract the first line of the code, change whitespaces to _
        first_line = code.split('\n')[0].replace(' ', '_')

        steps.append('extracted first line')

        # filename is the type + "_" + first_line + random number from 1 to 10000
        filename = f'{type}_{first_line}_{str(random.randint(1, 10000))}'

        steps.append('created filename')

        # extract embeddings from the code
        if model == 'codebert':
            snippet_embeddings = codebert_embeddings.extract_embeddings_codebert_from_string(code)
        elif model == 'singberta':
            snippet_embeddings = singberta_embeddings.extract_embeddings_singberta_from_string(code)
        
        steps.append('extracted embeddings')

        # read the db in csv
        dfDB = pd.read_csv(f'database/embeddings_{model}.csv', sep='$')

        steps.append('read reference database, rows = ' + str(dfDB.shape[0]) + ', columns = ' + str(dfDB.shape[1]))

        # add the embeddings to the database
        df = pd.DataFrame([snippet_embeddings])
        dfDBEmbeddings = dfDB.drop(['filename', 'vulnerability'], axis=1)

        df.columns = dfDBEmbeddings.columns
        
        df['filename'] = filename
        df['vulnerability'] = vulnerability

        steps.append('created dataframe rows ' + str(df.shape[0]) + ' columns ' + str(df.shape[1]))

        steps.append(f'DB: {len(dfDB.columns)}')
        steps.append(f'df: {len(df.columns)}')



        # now update that row with the values of the snippet embeddings list
        dfDB = dfDB.append(df, ignore_index=True)

        steps.append('concatenated dataframes rows = ' + str(dfDB.shape[0]) + ' columns = ' + str(dfDB.shape[1]))

        # save the dataframe to the csv
        dfDB.to_csv(f'database/embeddings_{model}.csv', sep='$', index=False)

        steps.append('saved dataframe')
        
        # send a response where we just concatenate all input into one string
        return flask.jsonify({'result': f'Added: {code}', 
                              'trace': steps})
    
    except Exception as e:
        print(f"Exception message: {str(e)}")
        return flask.jsonify({'result': f'Error: {str(e)}', 'references': []})

# define the standard endpoint at / that displays a webpage from the web foldes
@app.route('/')
def index():
    # the template is in the web folder
    return flask.render_template('index.html')


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001, debug=True)
