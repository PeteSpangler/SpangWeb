from flask import Flask, render_template, request


app = Flask(__name__)


@app.route('/')
def hello():
    return render_template("index.html")

@app.route('/about')
def about():
    return render_template("page2.html")

@app.route('/textured_glass')
def textured_glass():
    return render_template("textured_glass.html")

@app.route('/randomthoughts')
def randomthoughts():
    return render_template("randomthoughts.html")

if __name__ == '__main__':
    app.run()
