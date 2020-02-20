# ASP.NET Core with Elasticsearch + Kibana

A sample of how to integrate ASP.NET Core with Elasticsearch.

## Elasticsearch and Kibana

Go to the **ElasticDocker** folder and type the command below

docker-compose up -d

> Wait a minute or 2 for Elastic and Kibana to start up

To check that everything is working correctly:

**Elasticsearch**

http://localhost:9200 **Or** http://localhost:9200/_cat/indices?v

**Kibana**

http://localhost:5601
