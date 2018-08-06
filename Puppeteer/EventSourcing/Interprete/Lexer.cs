using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete
{


    using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;
    using Meses = Puppeteer.EventSourcing.Libraries.Meses;
    using Monedas = Puppeteer.EventSourcing.Libraries.Monedas;
    using TokenType = Puppeteer.EventSourcing.Interprete.Token.TokenType;

    public class Lexer
	{
		public Token tokenActual;
		private Entrada entrada;

        public Lexer(string codigo)
		{
			entrada = new Entrada(this);
            entrada.Establecer(codigo);
			ObtenerSiguiente();
		}

		public virtual string Comando
		{
			set
			{
				value += " \f";
				entrada.Establecer(value);
				ObtenerSiguiente();
			}
		}

		private void ObtenerSiguiente()
		{
			while (true)
			{
				try
				{
					entrada.LimpiarParaLeerOtroToken();
					EliminarEspacios();
					if (EsNumero())
					{
						entrada.AvanceGuardandoCaracter();
						if (EsNumero()) //00..
						{
							entrada.AvanceGuardandoCaracter();
							if (entrada.caracterActual == ':') //hora HH:
							{
								entrada.AvanceGuardandoCaracter();
								if (EsNumero())
								{
									entrada.AvanceGuardandoCaracter();
									if (EsNumero())
									{
										entrada.AvanceGuardandoCaracter();
										if (entrada.caracterActual == ':') //HH:MM:
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero())
											{
												entrada.AvanceGuardandoCaracter();
												if (EsNumero())
												{
													entrada.AvanceGuardandoCaracter();
													if (EsUnFinalDeNumero()) //HH:MM:SS<eol>
													{
														tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
														break;
													}

													entrada.DevolverCaracter();
												}
												else if (EsUnFinalDeNumero()) //HH:MM:S<eol>
												{
													tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
													break;
												}
												entrada.DevolverCaracter();
											}
											entrada.DevolverCaracter();
										}
										entrada.DevolverCaracter();
									}
									else if (entrada.caracterActual == ':') // HH:M:
									{
										entrada.AvanceGuardandoCaracter();
										if (EsNumero()) //HH:M:S
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero()) //HH:M:SS
											{
												entrada.AvanceGuardandoCaracter();
												if (EsUnFinalDeNumero())
												{
													tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
													break;
												}
											}
											else if (EsUnFinalDeNumero()) //HH:M:S<eol>
											{
												tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
												break;
											}
										}
										entrada.DevolverCaracter();
									}
									entrada.DevolverCaracter();
								}
								entrada.DevolverCaracter();
							}
							else if (EsUnDividir()) // DD/
							{
								entrada.AvanceGuardandoCaracter();
								if (EsNumero())
								{
									entrada.AvanceGuardandoCaracter();
									if (EsNumero())
									{
										entrada.AvanceGuardandoCaracter();
										if (EsUnDividir()) //DD/MM/
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero())
											{
												entrada.AvanceGuardandoCaracter();
												if (EsNumero())
												{
													entrada.AvanceGuardandoCaracter();
													if (EsNumero())
													{
														entrada.AvanceGuardandoCaracter();
														if (EsNumero())
														{
															entrada.AvanceGuardandoCaracter();
															if (EsUnFinalDeNumero())
															{
																tokenActual = new Token(TokenType.fecha, entrada.CadenaActual());
																break;
															}
															entrada.DevolverCaracter();
														}
														entrada.DevolverCaracter();
													}
													entrada.DevolverCaracter();
												}
												entrada.DevolverCaracter();
											}
											entrada.DevolverCaracter();
										}
										entrada.DevolverCaracter();
									}
									else if (EsUnDividir()) //DD/M/
									{
										entrada.AvanceGuardandoCaracter();
										if (EsNumero())
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero())
											{
												entrada.AvanceGuardandoCaracter();
												if (EsNumero())
												{
													entrada.AvanceGuardandoCaracter();
													if (EsNumero())
													{
														entrada.AvanceGuardandoCaracter();
														if (EsUnFinalDeNumero())
														{
															tokenActual = new Token(TokenType.fecha, entrada.CadenaActual());
															break;
														}
														entrada.DevolverCaracter();
													}
													entrada.DevolverCaracter();
												}
												entrada.DevolverCaracter();
											}
											entrada.DevolverCaracter();
										}
										entrada.DevolverCaracter();
									}
									entrada.DevolverCaracter();
								}
								entrada.DevolverCaracter();
							}
							entrada.DevolverCaracter();
						}
						else if (EsUnDividir()) //0...
						{
							entrada.AvanceGuardandoCaracter();
							if (EsNumero())
							{
								entrada.AvanceGuardandoCaracter();
								if (EsNumero()) //D/MM
								{
									entrada.AvanceGuardandoCaracter();
									if (EsUnDividir())
									{
										entrada.AvanceGuardandoCaracter();
										if (EsNumero())
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero())
											{
												entrada.AvanceGuardandoCaracter();
												if (EsNumero())
												{
													entrada.AvanceGuardandoCaracter();
													if (EsNumero())
													{
														entrada.AvanceGuardandoCaracter();
														if (EsUnFinalDeNumero())
														{
															tokenActual = new Token(TokenType.fecha, entrada.CadenaActual());
															break;
														}
														entrada.DevolverCaracter();
													}
													entrada.DevolverCaracter();
												}
												entrada.DevolverCaracter();
											}
											entrada.DevolverCaracter();
										}
										entrada.DevolverCaracter();
									}
									entrada.DevolverCaracter();
								}
								else if (EsUnDividir()) //D/M/..
								{
									entrada.AvanceGuardandoCaracter();
									if (EsNumero())
									{
										entrada.AvanceGuardandoCaracter();
										if (EsNumero())
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero())
											{
												entrada.AvanceGuardandoCaracter();
												if (EsNumero())
												{
													entrada.AvanceGuardandoCaracter();
													if (EsUnFinalDeNumero())
													{
														tokenActual = new Token(TokenType.fecha, entrada.CadenaActual());
														break;
													}
													entrada.DevolverCaracter();
												}
												entrada.DevolverCaracter();
											}
											entrada.DevolverCaracter();
										}
										entrada.DevolverCaracter();
									}
									entrada.DevolverCaracter();
								}
								entrada.DevolverCaracter();
							}
							entrada.DevolverCaracter();
						}
						else if (entrada.caracterActual == ':') // H:
						{
							entrada.AvanceGuardandoCaracter();
							if (EsNumero())
							{
								entrada.AvanceGuardandoCaracter();
								if (EsNumero()) //H:MM
								{
									entrada.AvanceGuardandoCaracter();
									if (entrada.caracterActual == ':') //H:MM:
									{
										entrada.AvanceGuardandoCaracter();
										if (EsNumero()) //H:MM:S
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero()) //H:MM:SS
											{
												entrada.AvanceGuardandoCaracter();
												if (EsUnFinalDeNumero())
												{
													tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
													break;
												}
											}
											else if (EsUnFinalDeNumero()) //H:MM:S
											{
												tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
												break;
											}
										}
										entrada.DevolverCaracter();
									}
									entrada.DevolverCaracter();
								}
								else if (entrada.caracterActual == ':') //H:N:
								{
									entrada.AvanceGuardandoCaracter();
									if (EsNumero()) //H:M:S
									{
										entrada.AvanceGuardandoCaracter();
										if (EsNumero()) //H:MM:SS
										{
											entrada.AvanceGuardandoCaracter();
											if (EsUnFinalDeNumero())
											{
												tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
												break;
											}
										}
										else if (EsUnFinalDeNumero()) //H:M:S
										{
											tokenActual = new Token(TokenType.hora, entrada.CadenaActual());
											break;
										}
									}
									entrada.DevolverCaracter();
								}
							}
							entrada.DevolverCaracter();
						}

						bool esDecimal = ProcesarNumero();
						if (esDecimal)
						{
							tokenActual = new Token(TokenType.@decimal, entrada.CadenaActual());
							break;
						}
						tokenActual = new Token(TokenType.numero, entrada.CadenaActual());
						break;
					}
					else if (EsUnCaracterDeId())
					{
						entrada.AvanceGuardandoCaracter();
						entrada.AvanceGuardandoCaracter();
						entrada.AvanceGuardandoCaracter();

						if (EsAlgunaMoneda())
						{
							if (EsEspacio())
							{
								entrada.AvanceGuardandoCaracter();
								if (EsNumero())
								{
									ProcesarMonto();
									if (EsUnFinalDeNumero())
									{
										tokenActual = new Token(TokenType.monto, entrada.CadenaActual());
										break;
									}
									entrada.DevolverCaracter();
								}
								entrada.DevolverCaracter();
							}
							else if (EsNumero())
							{
								ProcesarMonto();
								if (EsUnFinalDeNumero())
								{
									tokenActual = new Token(TokenType.monto, entrada.CadenaActual());
									break;
								}
								entrada.DevolverCaracter();
							}
						}
						entrada.DevolverCaracter();
						entrada.DevolverCaracter();
						entrada.DevolverCaracter();

						ProcesarIdentificador();

						string cadenaActualOriginal = entrada.CadenaActual();
						string cadenaActualUnsensitive = cadenaActualOriginal.ToUpper();
						if (cadenaActualUnsensitive.Equals("PRINT"))
						{
							tokenActual = new Token(TokenType.print, cadenaActualOriginal);
						}
						else if (Meses.contieneElMes(cadenaActualUnsensitive))
						{
							if (EsUnDividir())
							{
								entrada.AvanceGuardandoCaracter();
								if (EsNumero())
								{
									entrada.AvanceGuardandoCaracter();
									if (EsNumero())
									{
										entrada.AvanceGuardandoCaracter();
										if (EsNumero())
										{
											entrada.AvanceGuardandoCaracter();
											if (EsNumero())
											{
												entrada.AvanceGuardandoCaracter();
												if (EsUnFinalDeNumero())
												{
													tokenActual = new Token(TokenType.mes, entrada.CadenaActual());
													break;
												}
											}
										}
									}
								}
							}

							throw new LanguageException(entrada.CadenaActual() + " es una palabra reservada del lenguaje", cadenaActualOriginal, entrada.fila, entrada.columna);
						}
						else if (cadenaActualUnsensitive.Equals("TRUE"))
						{
							tokenActual = new Token(TokenType.boolTrue, cadenaActualOriginal);
						}
						else if (cadenaActualUnsensitive.Equals("FALSE"))
						{
							tokenActual = new Token(TokenType.boolFalse, cadenaActualOriginal);
						}
						else if (cadenaActualUnsensitive.Equals("IF"))
						{
							tokenActual = new Token(TokenType.IF, cadenaActualOriginal);
						}
						else if (cadenaActualUnsensitive.Equals("ELSE"))
						{
							tokenActual = new Token(TokenType.ELSE, cadenaActualOriginal);
						}
                        else if (cadenaActualUnsensitive.Equals("EVAL"))
                        {
                            tokenActual = new Token(TokenType.EVAL, cadenaActualOriginal);
                        }
                        else if (cadenaActualUnsensitive.Equals("NULL"))
						{
							tokenActual = new Token(TokenType.nulo, cadenaActualOriginal);
						}
						else if (cadenaActualUnsensitive.Equals("PROCEDURE"))
						{
							tokenActual = new Token(TokenType.procedure, cadenaActualOriginal);
						}
						else if (cadenaActualUnsensitive.Equals("AS"))
						{
							tokenActual = new Token(TokenType.@as, cadenaActualOriginal);
						}
                        else if (cadenaActualUnsensitive.Equals("FOR"))
                        {
                            tokenActual = new Token(TokenType.FOR, cadenaActualOriginal);
                        }
                        else
						{
							tokenActual = new Token(TokenType.id, cadenaActualOriginal);
						}
						break;
					}
					else
					{
						switch (entrada.caracterActual)
						{
					case '\'' :
					case '\"' :
						ProcesarLiteralString();
						tokenActual = new Token(TokenType.hilera, entrada.CadenaActual());
						return;
					case '.' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.punto, ".");
						return;
					case '+' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.suma, "+");
						return;
					case '-' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.resta, "-");
						return;
					case '/':
						entrada.AvanceGuardandoCaracter();
						bool esComentarioDeLinea = entrada.caracterActual == '/';
						if (esComentarioDeLinea)
						{
							entrada.AvanceGuardandoCaracter();
							ProcesarComentario();
							tokenActual = new Token(TokenType.comentarioDeLinea, entrada.CadenaActual());
						}
						else
						{
							tokenActual = new Token(TokenType.division, "/");
						}
						return;
					case '>' :
						entrada.AvanceCaracterSinGuardar();
						if (entrada.caracterActual == '=')
						{
							entrada.AvanceCaracterSinGuardar();
							tokenActual = new Token(TokenType.mayorIgual, ">=");
							return;
						}
						tokenActual = new Token(TokenType.mayor, ">");
						return;
					case '<' :
						entrada.AvanceCaracterSinGuardar();
						if (entrada.caracterActual == '=')
						{
							entrada.AvanceCaracterSinGuardar();
							tokenActual = new Token(TokenType.menorIgual, "<=");
							return;
						}

						tokenActual = new Token(TokenType.menor, "<");
						return;
					case '=' :
						entrada.AvanceCaracterSinGuardar();
						if (entrada.caracterActual == '=')
						{
							entrada.AvanceCaracterSinGuardar();
							tokenActual = new Token(TokenType.igualdad, "==");
							return;
						}

						tokenActual = new Token(TokenType.igual, "=");
						return;
					case '&' :
						entrada.AvanceCaracterSinGuardar();
						if (entrada.caracterActual == '&')
						{
							entrada.AvanceCaracterSinGuardar();
							tokenActual = new Token(TokenType.yLogico, "&&");
							return;
						}
						throw new LanguageException("Error de sintaxis en el caracter &", entrada.caracterActual + "", entrada.fila, entrada.columna);
					case '|' :
						entrada.AvanceCaracterSinGuardar();
						if (entrada.caracterActual == '|')
						{
							entrada.AvanceCaracterSinGuardar();
							tokenActual = new Token(TokenType.oLogico, "&&");
							return;
						}
						throw new LanguageException("Error de sintaxis en el caracter |", entrada.caracterActual + "", entrada.fila, entrada.columna);
					case '!' :
						entrada.AvanceCaracterSinGuardar();
						if (entrada.caracterActual == '=')
						{
							entrada.AvanceCaracterSinGuardar();
							tokenActual = new Token(TokenType.desigualdad, "!=");
							return;
						}
						tokenActual = new Token(TokenType.negacionLogica, "!");
						return;
					case '*' :
						entrada.AvanceGuardandoCaracter();
						tokenActual = new Token(TokenType.multiplicacion, "*");
						return;
					case '}' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.end, "}");
						return;
					case '{' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.begin, "{");
						return;
					case '(' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.lParentesis, "(");
						return;
					case ')' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.rParentesis, ")");
						return;
                    case ':':
                        entrada.AvanceCaracterSinGuardar();
                        tokenActual = new Token(TokenType.dosPuntos, ":");
                        return;
                    case ',' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.coma, ",");
						return;
					case ';' :
						entrada.AvanceCaracterSinGuardar();
						tokenActual = new Token(TokenType.puntoComa, ";");
						return;
					case '\f' :
						tokenActual = new Token(TokenType.eof, "<eof>");
						return;
					case '\n' :
						entrada.AvanceCaracterSinGuardar();
						continue;
					case '\r' :
						entrada.AvanceCaracterSinGuardar();
						if (entrada.caracterActual == '\n')
						{
							entrada.AvanceCaracterSinGuardar();
						}
						continue;
						}
					}
				}
				catch (Exception e)
				{
					if (e is LanguageException)
					{
						throw (LanguageException)e;
					}
					throw new LanguageException(" El caracter " + entrada.caracterActual + " es inválido.", entrada.CadenaActual(), entrada.fila, entrada.columna);
				}

				throw new LanguageException("La linea presenta caracteres inválidos.", entrada.CadenaActual(), entrada.fila, entrada.columna);
			}
		}

		private void ProcesarComentario()
		{
			while (!EsFinalDeComando())
			{
				entrada.AvanceGuardandoCaracter();
			}
		}

		private bool EsAlgunaMoneda()
		{
			bool esAlgunaMoneda = Monedas.contieneLaMoneda(entrada.CadenaActual());
			return esAlgunaMoneda;
		}

		public virtual void Aceptar()
		{
			ObtenerSiguiente();
		}

		public virtual void Aceptar(TokenType tipo)
		{
			TokenType currentType = tokenActual.Type;
			if (currentType != tipo)
			{
				throw new LanguageException(string.Format("Se esperaba un '{0}' y se encontró el valor '{1}' de tipo '{2}'.", tipo.ToString(), tokenActual.Valor, currentType.ToString()), entrada.CadenaActual(), entrada.fila, entrada.columna);
			}
			Aceptar();
		}

		private bool ProcesarNumero()
		{
			bool esDecimal = false;
			while (EsNumero() || EsPunto())
			{

				if (EsPunto())
				{
					if (esDecimal)
					{
						throw new LanguageException("se encontro mas de 1 punto en el numero", entrada.CadenaActual(), entrada.fila, entrada.columna);
					}
					else
					{
						esDecimal = true;
					}
				}
				entrada.AvanceGuardandoCaracter();

			}
			if (EsPorcentaje())
			{
				entrada.AvanceCaracterSinGuardar();
			}
			if (entrada.caracterActual == '.')
			{
				throw new LanguageException("se encontro un punto al final del numero", entrada.CadenaActual(), entrada.fila, entrada.columna);
			}

			return esDecimal;
		}

		private bool ProcesarMonto()
		{
			bool esDecimal = false;
			int cantidadDeDecimales = 0;
			while (EsNumero() || EsPunto())
			{
				if (EsPunto())
				{
					if (esDecimal)
					{
						throw new LanguageException("se encontro mas de 1 punto en el numero", entrada.CadenaActual(), entrada.fila, entrada.columna);
					}
					else
					{
						esDecimal = true;
					}
				}
				else
				{
					if (esDecimal)
					{
						cantidadDeDecimales++;
					}
				}
				entrada.AvanceGuardandoCaracter();
			}
			if (EsPorcentaje())
			{
				entrada.AvanceCaracterSinGuardar();
			}
			if (entrada.caracterActual == '.')
			{
				throw new LanguageException("se encontro un punto al final del numero", entrada.CadenaActual(), entrada.fila, entrada.columna);
			}
			if (cantidadDeDecimales > 0 && cantidadDeDecimales != 2)
			{
				throw new LanguageException("Los montos de dinero solo pueden tener 0 o 2 decimales", entrada.CadenaActual(), entrada.fila, entrada.columna);
			}

			return esDecimal;
		}

		private void ProcesarIdentificador()
		{
			entrada.AvanceGuardandoCaracter();
			while (true)
			{
				if (EsUnCaracterDeId() || EsNumero())
				{
					entrada.AvanceGuardandoCaracter();
				}
				else
				{
					break;
				}
			}
		}

		private void ProcesarLiteralString()
		{
			char comillaInicial = entrada.caracterActual;
			while (true)
			{
				entrada.AvanceGuardandoCaracter();

				if (EsBackSlash())
				{
					entrada.AvanceCaracterSinGuardar();
				}
				else if (entrada.caracterActual == comillaInicial)
				{
					entrada.AvanceGuardandoCaracter();
					break;
				}
			}
		}

		private static readonly string CARACTERES_VALIDOS = new string(new char[]{'"', ';', '=', ':', ',', '(', ')', '+', '\'', '/', '*', '-', '>', '<', '!', '{', '}', '%', '.', '&', '|'});

        private void EliminarEspacios()
		{
			while (true)
			{
				bool esFinDeArchivo_o_noEsUnEspacio = char.IsLetterOrDigit(entrada.caracterActual) || CARACTERES_VALIDOS.IndexOf(entrada.caracterActual) >= 0 || entrada.caracterActual == '\f';
				if (esFinDeArchivo_o_noEsUnEspacio)
				{
					break;
				}
				else
				{
					entrada.AvanceCaracterSinGuardar();
				}
			}
		}

		private bool EsMultiplicacion()
		{
			bool esUnMultiplicacion = entrada.caracterActual == '*';
			return esUnMultiplicacion;
		}

		private bool EsUnDividir()
		{
			bool esUnDividir = entrada.caracterActual == '/';
			return esUnDividir;
		}

		private bool EsPunto()
		{
			bool esUnPunto = entrada.caracterActual == '.';
			return esUnPunto;
		}

		private bool EsNumero()
		{
			bool esUnNumero = char.IsDigit(entrada.caracterActual);
			return esUnNumero;
		}

		internal virtual bool EsEspacio()
		{
			bool esUnEspacio = entrada.caracterActual == ' ' || entrada.caracterActual == '\t';
			return esUnEspacio;
		}

		private static readonly string OPERADORES = new string(new char[]{'=', '+', '-', '*', '<', '>', '!', '/'});
		private static readonly string FIN_DE_NUMERO = new string(new char[]{',', ')', '}', ';', '%'});
		private bool EsUnFinalDeNumero()
		{
			bool esUnFinalDeNumero = EsEspacio() || OPERADORES.IndexOf(entrada.caracterActual) >= 0 || FIN_DE_NUMERO.IndexOf(entrada.caracterActual) >= 0 || EsFinalDeComando();
			return esUnFinalDeNumero;
		}

		private bool EsFinalDeComando()
		{
			bool esElFinalDelComando = entrada.caracterActual == '\n' || entrada.caracterActual == '\r' || entrada.caracterActual == '\f';
			return esElFinalDelComando;
		}

		private bool EsPorcentaje()
		{
			bool esUnPorcentaje = entrada.caracterActual == '%';
			return esUnPorcentaje;
		}

		private bool EsBackSlash()
		{
			bool esBackSlash = entrada.caracterActual == '\\';
			return esBackSlash;
		}

		private bool EsUnCaracterDeId()
		{
			char character = entrada.caracterActual;
			bool esLetra = char.IsLetter(character);
			if (esLetra)
			{
				return true;
			}
			bool esGuionBajo = character == '_';
			bool esNumeral = character == '#';
			bool esArroba = character == '@';
			return esGuionBajo || esNumeral || esArroba;
		}

		public virtual int Fila()
		{
			return entrada.fila;
		}

		public virtual int Columna()
		{
			return entrada.columna;
		}

		private class Entrada
		{
			internal bool InstanceFieldsInitialized = false;

			internal virtual void InitializeInstanceFields()
			{
				posiciones = new Posiciones(outerInstance, this);
			}

			private readonly Lexer outerInstance;

			public Entrada(Lexer outerInstance)
			{
				this.outerInstance = outerInstance;

				if (!InstanceFieldsInitialized)
				{
					InitializeInstanceFields();
					InstanceFieldsInitialized = true;
				}
			}

			public char caracterActual;

			internal char[] arregloCaracteres;
			internal int indiceActual = 0;
			internal int fila;
			internal int columna;
			internal StringBuilder cadenaActual_Renamed = new StringBuilder();

			internal Posiciones posiciones;

			internal virtual void Establecer(string entrada)
			{
				arregloCaracteres = entrada.ToCharArray();
				Limpiar();
			}

			internal virtual void Limpiar()
			{
				caracterActual = ' ';
				indiceActual = 0;
				fila = 1;
				columna = 0;
				cadenaActual_Renamed.Length = 0;
			}

			internal virtual void LimpiarParaLeerOtroToken()
			{
				cadenaActual_Renamed.Length = 0;
				posiciones.LimpiarParaLeerOtroToken();
			}

			internal virtual void AnotarElDesplazamientoDeLaLinea(bool guardando)
			{
				fila++;
				columna = 0;
				Avanzar(guardando);
			}

			internal virtual void AnotarElDesplazamientoDeLaColumna(bool guardando)
			{
				columna++;
				Avanzar(guardando);
			}

			internal virtual void Avanzar(bool guardando)
			{
				if (guardando)
				{
					posiciones.GuardarPosicion();
					cadenaActual_Renamed.Append(caracterActual);
				}
				caracterActual = arregloCaracteres[indiceActual];
				indiceActual++;
			}

			internal virtual void AvanceGuardandoCaracter()
			{
				switch (caracterActual)
				{
					case '\n':
						outerInstance.entrada.AnotarElDesplazamientoDeLaLinea(true);
						break;
					case '\f':
						throw new LanguageException("EOF inesperado", outerInstance.entrada.cadenaActual_Renamed.ToString(), outerInstance.entrada.fila, outerInstance.entrada.columna);
					default:
						outerInstance.entrada.AnotarElDesplazamientoDeLaColumna(true);
					break;
				}
			}

			internal virtual void AvanceCaracterSinGuardar()
			{
				switch (caracterActual)
				{
					case '\n':
						outerInstance.entrada.AnotarElDesplazamientoDeLaLinea(false);
						break;
					case '\f':
						throw new LanguageException("EOF inesperado", outerInstance.entrada.cadenaActual_Renamed.ToString(), outerInstance.entrada.fila, outerInstance.entrada.columna);
					default:
						outerInstance.entrada.AnotarElDesplazamientoDeLaColumna(false);
					break;
				}
			}

			internal virtual void DevolverCaracter()
			{
				indiceActual = posiciones.IndiceActual;
				columna = posiciones.Columna;
				fila = posiciones.Fila;
				posiciones.QuitarLaUltimaPosicion();
				cadenaActual_Renamed.Length = cadenaActual_Renamed.Length - 1;
				caracterActual = arregloCaracteres[indiceActual - 1];
			}

			internal virtual string CadenaActual()
			{
				return cadenaActual_Renamed.ToString().Trim();
			}

			internal virtual char[] ChartSetConflictivo()
			{
				int punteroDelCaracterAIncluir = (new string(arregloCaracteres)).LastIndexOf(";", StringComparison.Ordinal) + 1;

				int tamanno = arregloCaracteres.Length - punteroDelCaracterAIncluir;
				char[] chars = new char[tamanno];

				for (int i = 0; i < tamanno; i++)
				{
					chars[i] = arregloCaracteres[punteroDelCaracterAIncluir];
					punteroDelCaracterAIncluir++;
				}
				return chars;
			}
		}

		private class Posiciones
		{
			private readonly Lexer outerInstance;

			internal Entrada entradaActual;

			internal const int MAX_TAMANO_DE_UN_LEXEMA = 1024;
			internal int[] fila = new int[MAX_TAMANO_DE_UN_LEXEMA];
			internal int[] columna = new int[MAX_TAMANO_DE_UN_LEXEMA];
			internal int[] indices = new int[MAX_TAMANO_DE_UN_LEXEMA];
			internal int indice = -1;

			internal Posiciones(Lexer outerInstance, Entrada entrada)
			{
				this.outerInstance = outerInstance;
				this.entradaActual = entrada;
				GuardarPosicion();
			}

			internal virtual void LimpiarParaLeerOtroToken()
			{
				indice = -1;
				GuardarPosicion();
			}

			internal virtual void GuardarPosicion()
			{
				indice++;
				this.fila[indice] = entradaActual.fila;
				this.columna[indice] = entradaActual.columna;
				this.indices[indice] = entradaActual.indiceActual;
			}

			internal virtual void QuitarLaUltimaPosicion()
			{
				indice--;
			}

			internal virtual int Fila
			{
				get
				{
					return this.fila[indice];
				}
			}

			internal virtual int Columna
			{
				get
				{
					return this.columna[indice];
				}
			}

			internal virtual int IndiceActual
			{
				get
				{
					return this.indices[indice];
				}
			}
		}

	}

}