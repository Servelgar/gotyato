using Godot;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

public partial class DialogueSystem : Control
{
	[Export] public int node = 0 ; 
	[Export] public int initialNode = 0;

	RichTextLabel txt;
	public Dictionary<string,List<string>> extracto;
	XmlDocument doc = new XmlDocument();
	RichTextLabel speaker;
	[Export] bool isFinal = false;
	NinePatchRect caja;
	VBoxContainer cajaV;
	public int contPulsacionesBoton = 0;
	bool isOptions = false;
	List<Button> l_button = new List<Button>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
		//caja.Visible = false;
	 	LoadNodes();
		doc.Load("./dialogues/okhalam1.xml");
		extracto = extract_node(node,doc);
		txt.Text = extracto["text"][0];
		speaker.Text = extracto["speaker"][0];
		
		// node = extract_first_id(doc);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(extracto["speaker"].Count > 1){
				isFinal = true;
		}
		
		if(Input.IsActionJustPressed("ui_accept")){
			if(isFinal){
				Visible = false;
				node = initialNode;
				extracto = extract_node(node,doc);
				txt.Text = extracto["text"][0];
				speaker.Text = extracto["speaker"][0];
				isFinal = false;
				
			} else if(Visible) {
				mostrarTexto();
				
			}
		}
		if(isOptions){
			if(Input.IsActionJustPressed("1")){
				contPulsacionesBoton = 0;
				mostrarTexto();
			} else if(Input.IsActionJustPressed("2")) {
				 // TO-DO cambiar esto que ahroa mismo esta sucio y scrunkly para solo 2 elecciones
				 contPulsacionesBoton = 1;
				 mostrarTexto();
			}
		}
	}
	

	// Carga todos los nodos que se utilizaran
	void LoadNodes(){
		txt = GetNode<RichTextLabel>("NinePatchRect/Text");
		speaker = GetNode<RichTextLabel>("NinePatchRect/Speaker");
		caja = GetNode<NinePatchRect>("NinePatchRect/optionBox");
		cajaV = GetNode<VBoxContainer>("NinePatchRect/optionBox/ScrollContainer/CajaV");
	}

	void mostrarTexto(){
		node = siguienteNodo(extracto);
		extracto = extract_node(node,doc);
		txt.Text = extracto["text"][0];
		speaker.Text = extracto["speaker"][0];
		if(extracto["options_text"].Count > 0){
			// Cuando se muestra un nodo con opciones, se pone visible la caja de opciones
			// Y se recorre en bucle los hijos de la cajav y se displayea el texto 
			caja.Visible = true;
			isOptions = true;
			l_button = new List<Button>();
			for(int i = 0; i < extracto["options_text"].Count; i++){
				Button boton = new();
				l_button.Add(boton);
				boton.Text = extracto["options_text"][i];
				cajaV.AddChild(boton);
				boton.Pressed += () => contPulsacionesBoton = l_button.IndexOf(boton);
				boton.Pressed += ButtonPressed;
			}
			
		} else {
			caja.Visible = false;
			isOptions = false;
			foreach(Button b in cajaV.GetChildren().Cast<Button>())
			{
				cajaV.RemoveChild(b);
			}
		}
		
	}

	void ButtonPressed(){
		mostrarTexto();
	}

	//
	int siguienteNodo(Dictionary<string,List<string>> extracto){
		int res = node;
		if (!extracto["siguiente_nodos"].Any()){
			res++;
		} else {
			// AQUI UN MONTON DE CODIGO PARA CUANDO SE IMPLEMENTE LAS DECISIONES Y RAMIFICACIONES
			// LA LOGICA ES QUE SE LEA EL INPUT Y SE IMPORTE AQUÍ COMO SIGUIENTE NODO
			// Doesn't work if the list is not sorted!!!
			res = int_nodos_parser(extracto["siguiente_nodos"])[contPulsacionesBoton];			
		}

		return res;
	}

	static Dictionary<string,List<string>> extract_node(int nodo_actual, 
	XmlDocument doc){
			// Create an XmlDocument object and load the XML file
			Dictionary<string,List<string>> res = new Dictionary<string,List<string>>();
			res["speaker"] = new List<string>();
			res["text"]= new List<string>();
			res["options_text"]= new List<string>();
			res["siguiente_nodos"] = new List<string>();
			
			// Get the root element
		//	XmlNode root = doc.SelectSingleNode("root");
			XmlNode nodo = doc.SelectSingleNode("/root/node[@id="+nodo_actual+"]");
			string speaker = nodo.SelectSingleNode("speaker").InnerText;	
			string text = nodo.SelectSingleNode("text").InnerText;
			XmlNode options = nodo.SelectSingleNode("options");
			XmlNode final = nodo.SelectSingleNode("final");
			
			if (options != null){
				foreach (XmlNode node in options.ChildNodes){
					// Doesn't work if the list is not sorted 
					res["options_text"].Add(node.SelectSingleNode("body").InnerText);
					res["siguiente_nodos"].Add(node.Attributes["nextNode"].Value);
				}
			} 
			res["speaker"].Add(speaker);
			if(final != null){
				res["speaker"].Add("1");
			}
			res["text"].Add(text);			
			return res;
		}

	// Given a xml doc, extract the first id to begin dialogue process
	static int extract_first_id(XmlNode doc){
		XmlNode root = doc.SelectSingleNode("root");
		int res = 0;
		if (root.HasChildNodes){
			XmlNode primerNodo = root.FirstChild;
			if (primerNodo.Attributes["id"] == null){
				//Console.WriteLine("si");
			}
			res = Int32.Parse(primerNodo.Attributes["id"].Value);
		} 
		return res;
	}

	// given siguiente_nodos key and its values, parser them to integer
	static List<int> int_nodos_parser(List<string> siguiente_nodos){
		List<int> res = new List<int>();
		if(!siguiente_nodos.Any()){
			return res;
		} else {
			foreach(string nodo in siguiente_nodos){
			res.Add(Int32.Parse(nodo));
		}
			return res;
		}
	}
}
