import flask
import codebert_embeddings
import singberta_embeddings
import pandas as pd
from single_file_pipeline import calculate_distances
from single_file_pipeline import calculate_distances_from_dict
from single_file_pipeline import check_vulnerability
import random

app = flask.Flask(__name__)

# accept a JSON string as input with two fields: code and model
@app.route('/predict', methods=['POST'])
def predict():
    steps = []

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
            snippet_embeddings = codebert_embeddings.extract_embeddings_codebert_from_string(code) if model == 'codebert' else singberta_embeddings.extract_embeddings_singberta_from_string(code)

            steps.append(f'extracted embeddings from {model}')

            # now read the embeddings of the reference files from database/embeddings_codebert_input_validation.csv
            dfRefEmbeddings = pd.read_csv(f'database/embeddings_{model}.csv', sep='$', index_col=None) 

            steps.append('read reference embeddings: ' + str(dfRefEmbeddings.shape[0]) + ' rows ' + str(dfRefEmbeddings.shape[1]) + ' columns')
            
            # select only the rows that contain the vulnerability
            dfRefEmbeddings = dfRefEmbeddings[dfRefEmbeddings['vulnerability'] == vulnerability]

            steps.append('selected only the rows that contain the vulnerability')
            
            # if the number of rows in dfRefEmbeddings is 0, then the model is not supported
            if dfRefEmbeddings.shape[0] == 0:
                vulnerability_result = 'Error: vulnerability not supported'
                steps.append('vulnebility not supported: ' + vulnerability)
                return flask.jsonify({'result': vulnerability_result, 
                                    'trace': steps})    

            # change the reference embeddings to a dictionary
            print(dfRefEmbeddings.columns)
            dfRefEmbeddings.set_index('filename', inplace=True)

            dictReferenceEmbeddings = dfRefEmbeddings.drop(['vulnerability'], axis=1).T.to_dict('list')

            steps.append('created reference dictionary')

            # calculate the distances between the reference and analyzed code
            dfDistances = calculate_distances_from_dict(dictReferenceEmbeddings, {'ME': snippet_embeddings})

            steps.append('calculated distances')

            # check vulnerability
            vulnerability_result, lstReferences = check_vulnerability(dfDistances)

            steps.append('checked vulnerability')

            # return a JSON string
            return flask.jsonify({'result': vulnerability_result, 
                                  'references': lstReferences})
        else:
            return flask.jsonify({'result': 'Error: model not supported', 
                                  'trace': steps})
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

    # return a JSON string
    return df.groupby('vulnerability').count()['filename'].to_json()

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
