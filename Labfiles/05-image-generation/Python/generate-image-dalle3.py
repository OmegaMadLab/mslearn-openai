# Note: DALL-E 3 requires version 1.0.0 of the openai-python library or later
import os
from openai import AzureOpenAI
import json
from dotenv import load_dotenv

load_dotenv()

client = AzureOpenAI(
    api_version="2023-12-01-preview",
    azure_endpoint=os.getenv("AZURE_OAI_ENDPOINT"),
    api_key=os.getenv("AZURE_OAI_KEY"),
)

# Get prompt for image to be generated
prompt = input("\nEnter a prompt to request an image: ")


result = client.images.generate(
    model=os.getenv("AZURE_OAI_MODEL"), # the name of your DALL-E 3 deployment
    prompt=prompt,
    n=1
)

json_response = json.loads(result.model_dump_json())
print("\nResponse from DALL-E 3:")
print(json_response)
print("\nImage URL:")

image_url = json.loads(result.model_dump_json())['data'][0]['url']

print(image_url)