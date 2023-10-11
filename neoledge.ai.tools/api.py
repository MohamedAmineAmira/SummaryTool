# Using flask to make an api
# import necessary libraries and functions
from flask import Flask, jsonify, request
import requests
from cleaningFrenchText import cleaning_french_text
from cleaningEnglishText import cleaning_english_text
from summarizing import summarizing_french_text, summarizing_english_text
# creating a Flask app
app = Flask(__name__)
@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"
@app.route('/cleaning-french-text', methods=['POST'])
def cleaning_french_request():
    if request.method == 'POST':
        json_data = request.json  # Access JSON data from the request
        asp_net_api_url = json_data["Url"]
        json_data["PrepareText"] = cleaning_french_text(json_data["PlainText"])
        json_data["State"] = 2
        # Make a POST request to the ASP.NET API
        response = requests.put(asp_net_api_url, json=json_data, verify=False)
        # Check the response status code
        if response.status_code == 204:
            print("Request was successful")
            # You can access the response data using response.json() if it returns JSON data
            # For example, response_data = response.json()
        else:
            print(f"Request failed with status code: {response.status_code}")
            print(response.text)  # This will print the response content if available
            return
        return str(200)
    else:
        return "Unsupported content type", 400

@app.route('/cleaning-english-text', methods=['POST'])
def cleaning_english_request():
    if request.method == 'POST':
        json_data = request.json  # Access JSON data from the request
        asp_net_api_url = json_data["Url"]
        json_data["PrepareText"] = cleaning_english_text(json_data["PlainText"])
        json_data["State"] = 2
        # Make a POST request to the ASP.NET API
        response = requests.put(asp_net_api_url, json=json_data, verify=False)
        # Check the response status code
        if response.status_code == 204:
            print("Request was successful")
            # You can access the response data using response.json() if it returns JSON data
            # For example, response_data = response.json()
        else:
            print(f"Request failed with status code: {response.status_code}")
            print(response.text)  # This will print the response content if available
            return
        return str(200)
    else:
        return "Unsupported content type", 400

@app.route('/resume-french-text', methods=['POST'])
def process_french_request():
    # Check if the content type is JSON
    if request.method == 'POST':
        json_data = request.json  # Access JSON data from the request
        asp_net_api_url = json_data["Url"]
        json_data["ProcessText"] = summarizing_french_text(json_data["PrepareText"])
        json_data["State"] = 4
        # Make a POST request to the ASP.NET API
        response = requests.put(asp_net_api_url, json=json_data, verify=False)
        # Check the response status code
        if response.status_code == 204:
            print("Request was successful")
            # You can access the response data using response.json() if it returns JSON data
            # For example, response_data = response.json()
        else:
            print(f"Request failed with status code: {response.status_code}")
            print(response.text)  # This will print the response content if available
            return
        return str(200)
    else:
        return "Unsupported content type", 400

@app.route('/resume-english-text', methods=['POST'])
def process_english_request():
    # Check if the content type is JSON
    if request.method == 'POST':
        json_data = request.json  # Access JSON data from the request
        asp_net_api_url = json_data["Url"]
        json_data["ProcessText"] = summarizing_english_text(json_data["PrepareText"])
        json_data["State"] = 4
        # Make a POST request to the ASP.NET API
        response = requests.put(asp_net_api_url, json=json_data, verify=False)

        # Check the response status code
        if response.status_code == 204:
            print("Request was successful")
            # You can access the response data using response.json() if it returns JSON data
            # For example, response_data = response.json()
        else:
            print(f"Request failed with status code: {response.status_code}")
            print(response.text)  # This will print the response content if available
            return
        return str(200)
    else:
        return "Unsupported content type", 400

# driver function
if __name__ == '__main__':
    app.run(debug=True)