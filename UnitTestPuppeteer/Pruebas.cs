using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puppeteer.EventSourcing.Interprete.Libraries;

namespace Puppeteer.UnitTest
{
    [TestClass]
    public class Pruebas
    {

        [TestMethod]
        public void ListComoTiposDeRetornoDeFuncion()
        {
            String resultado;

            resultado = Titere.Perform("C = Clase(); Print C.ListaDeInts() 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Lista\":[1,2,3]}}");

            resultado = Titere.Perform("C = Clase(); Print C.ListaDeBools() 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Lista\":[true,false,true]}}");

            resultado = Titere.Perform("C = Clase(); Print C.ListaDeDoubles() 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Lista\":[1.2,2.34,3.456]}}");

            resultado = Titere.Perform("C = Clase(); Print C.ListaDeStrings() 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Lista\":[\"AA\",\"BB\",\"Cc\"]}}");

            resultado = Titere.Perform("C = Clase(); Print C.ListaDeClase() 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Lista\":[{\"Clase\":\"AA\"},{\"Clase\":\"BB\"},{\"Clase\":\"CC\"}]}}");
        }

        [TestMethod]
        public void RecibirListComoParametro()
        {
            String resultado;

            resultado = Titere.Perform("C = Clase(); Print C.GooRecibirListaDeInts(C.ListaDeInts()) 'X';");
            Assert.AreEqual(resultado, "{\"X\":3}");

            resultado = Titere.Perform("C = Clase(); Print C.GooRecibirListaDeBools(C.ListaDeBools()) 'X';");
            Assert.AreEqual(resultado, "{\"X\":3}");

            resultado = Titere.Perform("C = Clase(); Print C.GooRecibirListaDeDoubles(C.ListaDeDoubles()) 'X';");
            Assert.AreEqual(resultado, "{\"X\":3}");

            resultado = Titere.Perform("C = Clase(); Print C.GooRecibirListaDeDecimals(C.ListaDeDecimals()) 'X';");
            Assert.AreEqual(resultado, "{\"X\":3}");

            resultado = Titere.Perform("C = Clase(); Print C.GooRecibirListaDeStrings(C.ListaDeStrings()) 'X';");
            Assert.AreEqual(resultado, "{\"X\":3}");

            resultado = Titere.Perform("C = Clase(); Print C.GooRecibirListaDeClase(C.ListaDeClase()) 'X';");
            Assert.AreEqual(resultado, "{\"X\":3}");

            resultado = Titere.Perform("C = Clase(); C2 = Clase(C.ListaDeStrings()); Print C2 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"Para Constructor con List<string>\"}}");

            resultado = Titere.Perform("C = Clase(); C2 = Clase(C.ListaDeInts()); Print C2 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"Para Constructor con List<int>\"}}");

            resultado = Titere.Perform("C = Clase(); C2 = Clase(C.ListaDeDoubles()); Print C2 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"Para Constructor con List<double>\"}}");

            resultado = Titere.Perform("C = Clase(); C2 = Clase(C.ListaDeBools()); Print C2 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"Para Constructor con List<bool>\"}}");

            resultado = Titere.Perform("C = Clase(); C2 = Clase(C.ListaDeClase()); Print C2 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"Para Constructor con List<Clase>\"}}");
        }

        [TestMethod]
        public void ImprimirLiterales()
        {
            String resultado;
            resultado = Titere.Perform("Print 1 'X';");
            Assert.AreEqual(resultado, "{\"X\":1}");
            resultado = Titere.Perform("Print 1.23 'X';");
            Assert.AreEqual(resultado, "{\"X\":1.23}");
            resultado = Titere.Perform("Print 'ABC' 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ABC\"}");
            resultado = Titere.Perform("Print 25/12/2018 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"25/12/2018\"}");
            resultado = Titere.Perform("Print 22/10/2019 10:11:12 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"22/10/2019 10:11:12\"}");
        }

        [TestMethod]
        public void SalidaDeForsSimples()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase(); for (v : C.ListaDeInts()) { print v 'valor'; } ");
            Assert.AreEqual(resultado, "{\"v\":[{\"valor\":1},{\"valor\":2},{\"valor\":3}]}");
            resultado = Titere.Perform("C = Clase(); for (v : C.ListaDeInts()) { a = 1; } ");
            Assert.AreEqual(resultado, "");
            resultado = Titere.Perform("C = Clase(); for (v : C.ListaDeInts()) { if (v == 1) Print v 'valor'; } ");
            Assert.AreEqual(resultado, "{\"v\":[{\"valor\":1}]}");
            resultado = Titere.Perform("C = Clase(); for (i : C.ListaDeInts()) { for (j : c.ListaDeInts()) { print i 'i'; print j 'j'; } } ");
            Assert.AreEqual(resultado, "{\"i\":[{\"j\":[{\"i\":1,\"j\":1},{\"i\":1,\"j\":2},{\"i\":1,\"j\":3}]},{\"j\":[{\"i\":2,\"j\":1},{\"i\":2,\"j\":2},{\"i\":2,\"j\":3}]},{\"j\":[{\"i\":3,\"j\":1},{\"i\":3,\"j\":2},{\"i\":3,\"j\":3}]}]}");
            resultado = Titere.Perform("C = Clase(); for (i : C.ListaDeInts()) { for (j : c.ListaDeInts()) { for (k : c.ListaDeInts()) {print i 'i'; print j 'j'; print k 'k'; } } } ");
            Assert.AreEqual(resultado, "{\"i\":[{\"j\":[{\"k\":[{\"i\":1,\"j\":1,\"k\":1},{\"i\":1,\"j\":1,\"k\":2},{\"i\":1,\"j\":1,\"k\":3}]},{\"k\":[{\"i\":1,\"j\":2,\"k\":1},{\"i\":1,\"j\":2,\"k\":2},{\"i\":1,\"j\":2,\"k\":3}]},{\"k\":[{\"i\":1,\"j\":3,\"k\":1},{\"i\":1,\"j\":3,\"k\":2},{\"i\":1,\"j\":3,\"k\":3}]}]},{\"j\":[{\"k\":[{\"i\":2,\"j\":1,\"k\":1},{\"i\":2,\"j\":1,\"k\":2},{\"i\":2,\"j\":1,\"k\":3}]},{\"k\":[{\"i\":2,\"j\":2,\"k\":1},{\"i\":2,\"j\":2,\"k\":2},{\"i\":2,\"j\":2,\"k\":3}]},{\"k\":[{\"i\":2,\"j\":3,\"k\":1},{\"i\":2,\"j\":3,\"k\":2},{\"i\":2,\"j\":3,\"k\":3}]}]},{\"j\":[{\"k\":[{\"i\":3,\"j\":1,\"k\":1},{\"i\":3,\"j\":1,\"k\":2},{\"i\":3,\"j\":1,\"k\":3}]},{\"k\":[{\"i\":3,\"j\":2,\"k\":1},{\"i\":3,\"j\":2,\"k\":2},{\"i\":3,\"j\":2,\"k\":3}]},{\"k\":[{\"i\":3,\"j\":3,\"k\":1},{\"i\":3,\"j\":3,\"k\":2},{\"i\":3,\"j\":3,\"k\":3}]}]}]}");
            resultado = Titere.Perform("C = Clase(); for (i : C.ListaDeInts()) { print i 'i'; for (j : c.ListaDeInts()) { print j 'j'; } } ");
            Assert.AreEqual(resultado, "{\"i\":[{\"i\":1,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}]},{\"i\":2,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}]},{\"i\":3,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}]}]}");
            resultado = Titere.Perform("C = Clase(); for (i : C.ListaDeInts()) { for (j : c.ListaDeInts()) { print j 'j'; } print i 'i'; } ");
            Assert.AreEqual(resultado, "{\"i\":[{\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":1},{\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":2},{\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":3}]}");
            resultado = Titere.Perform("C = Clase(); for (i : C.ListaDeInts()) { print i 'i'; for (j : c.ListaDeInts()) { print j 'j'; } print i 'i'; } ");
            Assert.AreEqual(resultado, "{\"i\":[{\"i\":1,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":1},{\"i\":2,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":2},{\"i\":3,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":3}]}");
            resultado = Titere.Perform("C = Clase(); print 100 'antes'; for (i : C.ListaDeInts()) { print i 'i'; for (j : c.ListaDeInts()) { print j 'j'; } print i 'i'; } ");
            Assert.AreEqual(resultado, "{\"antes\":100,\"i\":[{\"i\":1,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":1},{\"i\":2,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":2},{\"i\":3,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":3}]}");
            resultado = Titere.Perform("C = Clase(); for (i : C.ListaDeInts()) { print i 'i'; for (j : c.ListaDeInts()) { print j 'j'; } print i 'i'; } ; print 200 'despues'; ");
            Assert.AreEqual(resultado, "{\"i\":[{\"i\":1,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":1},{\"i\":2,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":2},{\"i\":3,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":3}],\"despues\":200}");
            resultado = Titere.Perform("C = Clase(); print 100 'antes'; for (i : C.ListaDeInts()) { print i 'i'; for (j : c.ListaDeInts()) { print j 'j'; } print i 'i'; } ; print 200 'despues'; ");
            Assert.AreEqual(resultado, "{\"antes\":100,\"i\":[{\"i\":1,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":1},{\"i\":2,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":2},{\"i\":3,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":3}],\"despues\":200}");
            resultado = Titere.Perform("C = Clase(); print 100 'antes'; for (k : C.ListaDeInts()) { print k 'k'; }; print 333 'medio'; for (i : C.ListaDeInts()) { print i 'i'; for (j : c.ListaDeInts()) { print j 'j'; } print i 'i'; } ; print 200 'despues'; ");
            Assert.AreEqual(resultado, "{\"antes\":100,\"k\":[{\"k\":1},{\"k\":2},{\"k\":3}],\"medio\":333,\"i\":[{\"i\":1,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":1},{\"i\":2,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":2},{\"i\":3,\"j\":[{\"j\":1},{\"j\":2},{\"j\":3}],\"i\":3}],\"despues\":200}");
            resultado = Titere.Perform("c = Clase(); for (c : c.ListaDeInts()) { a = 1;};  print 1 'X';");
            Assert.AreEqual(resultado, "{\"X\":1}");
        }

        [TestMethod]
        public void GetDeMetodosDeObjetos()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase(); Print C.GetInt() 'X';");
            Assert.AreEqual(resultado, "{\"X\":7}");
            resultado = Titere.Perform("C = Clase(); Print C.GetIntConParametro(\"ABC\") 'X';");
            Assert.AreEqual(resultado, "{\"X\":7}");
            resultado = Titere.Perform("C = Clase(); Print C.GetString() 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ABC\"}");
            resultado = Titere.Perform("C = Clase(); Print C.GetDouble() 'X';");
            Assert.AreEqual(resultado, "{\"X\":1.23}");
            resultado = Titere.Perform("C = Clase(); Print C.GetBool() 'X';");
            Assert.AreEqual(resultado, "{\"X\":true}");
            resultado = Titere.Perform("C = Clase(); Print C.GetObjeto() 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"Para GetObjeto()\"}}");
            resultado = Titere.Perform("v = Clase(); c = v.GetObjetoParametro('abc'); print c 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"abc\"}}");
            resultado = Titere.Perform("v = Clase(); c = v.GetPublicMetodo('abc'); print c 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"abc\"}}");
            resultado = Titere.Perform("v = Clase(); c = v.GetInternalMetodo('abc'); print c 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"abc\"}}");
        }

        [TestMethod]
        public void GetDeFieldsDeObjetos()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase(); Print C.XX 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ABCXYZ\"}");
            resultado = Titere.Perform("C = Clase(); Print C.Xx 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ABCXYZ\"}");
            resultado = Titere.Perform("v = Clase(); c = v.InternalField; print c 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"internalField\"}}");
            resultado = Titere.Perform("v = Clase(); c = v.PublicField; print c 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"publicField\"}}");
            resultado = Titere.Perform("C = Clase();  Print C.FooPlayer('abc', C.Player) 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"PLAYER\"}}");
            resultado = Titere.Perform("C = Clase(); Print C.publicField.ListaDeObjetos() 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Lista\":[{\"Clase\":\"AA\"},{\"Clase\":\"BB\"},{\"Clase\":\"CC\"}]}}");

            Assert.ThrowsException<LanguageException>(
                () => Titere.Perform("C = Clase(); Print C.Zx 'X';")
            );
        }

        [TestMethod]
        public void GetDePropertiesDeObjetos()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase(); Print C.PropertyInt 'X';");
            Assert.AreEqual(resultado, "{\"X\":7}");
            resultado = Titere.Perform("C = Clase(); Print C.PropERTYInt 'X';");
            Assert.AreEqual(resultado, "{\"X\":7}");
            resultado = Titere.Perform("v = Clase(); c = v.GetInternalProperty; print c 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"GetInternalProperty\"}}");
            resultado = Titere.Perform("v = Clase(); c = v.GetPublicProperty; print c 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"GetPublicProperty\"}}");
            resultado = Titere.Perform("a = 1; Print 102 'antes'; Eval(' a'+'1'+' = 1; Print a1 X; '); print a1+2 'Y';");
            Assert.AreEqual(resultado, "{\"antes\":102,\"X\":1,\"Y\":3}");
        }

        [TestMethod]
        public void ComandoEval()
        {
            String resultado = null;
            resultado = Titere.Perform("a = 1; Eval(' a'+'1'+' = 1; Print a1 X; '); print a1+2 'Y';");
            Assert.AreEqual(resultado, "{\"X\":1,\"Y\":3}");

            resultado = Titere.Perform("a = 1; Eval(' a' + 1 + ' = 1; Print a1 X; '); print a1+2 'Y';");
            Assert.AreEqual(resultado, "{\"X\":1,\"Y\":3}");

            resultado = Titere.Perform("a = 1; Eval(' a' + a + ' = 1; Print a1 X; '); print a1+2 'Y';");
            Assert.AreEqual(resultado, "{\"X\":1,\"Y\":3}");

            resultado = Titere.Perform("a = 1; Eval(' a' + a + ' = 1; '); print a1+2 'Y';");
            Assert.AreEqual(resultado, "{\"Y\":3}");
        }

        [TestMethod]
        public void ContatenacionesDeHileras()
        {
            String resultado;
            resultado = Titere.Perform(" print 'AA'+'BB' 'Y';");
            Assert.AreEqual(resultado, "{\"Y\":\"AABB\"}");

            resultado = Titere.Perform(" print 'AA'+ 1 'Y';");
            Assert.AreEqual(resultado, "{\"Y\":\"AA1\"}");

            resultado = Titere.Perform(" print 'AA'+1+1 'Y';");
            //MAL Assert.AreEqual(resultado, "{\"Y\":\"AA11\"}");

            resultado = Titere.Perform(" print 'AA'+1+'BB' 'Y';");
            Assert.AreEqual(resultado, "{\"Y\":\"AA1BB\"}");

            resultado = Titere.Perform(" print 'AA'+1*2 'Y';");
            Assert.AreEqual(resultado, "{\"Y\":\"AA2\"}");

            /* MAL
             * String res = "AA" + 1 / 2+ "BB";
             * resultado = Titere.Perform(" print 'AA'+1/2+'BB' 'Y';");
             */
        }

        [TestMethod]
        public void GetDeTiposPrimitivos()
        {
            String resultado;
            resultado = Titere.Perform("v = Clase(); unTotal = v.Total(); print unTotal 'X';");
            Assert.AreEqual(resultado, "{\"X\":123.456}");
            resultado = Titere.Perform("C = Clase(); Print C.Fecha() 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"03/05/2018 13:43:59\"}");
            resultado = Titere.Perform("C = Clase(); print C.FechaHora 'hora';");
            Assert.AreEqual(resultado, "{\"hora\":\"05/28/2018 16:10:53\"}");
            resultado = Titere.Perform("C = Clase(); C.FechaHora = 11/12/2019 11:12:13; print C.FechaHora 'hora';");
            Assert.AreEqual(resultado, "{\"hora\":\"12/11/2019 11:12:13\"}");
        }

        [TestMethod]
        public void ObjetosConUUID()
        {
            String resultado;
            resultado = Titere.Perform("v = Clase(); print v == v 'X';");
            Assert.AreEqual(resultado, "{\"X\":true}");

            resultado = Titere.Perform("v = Clase(); print v.UUID == v.UUID 'X';");
            Assert.AreEqual(resultado, "{\"X\":true}");

            resultado = Titere.Perform("v = Clase(); t = Clase(); print v.UUID == t.UUID 'X';");
            Assert.AreEqual(resultado, "{\"X\":false}");
        }

        [TestMethod]
        public void PuntoConPunto()
        {

        }

        [TestMethod]
        public void LValues()
        {
            String resultado;
            resultado = Titere.Perform("lhv1 = 1; Print lhv1 'X';");
            Assert.AreEqual(resultado, "{\"X\":1}");
            resultado = Titere.Perform("lhv2 = 'ABC'; Print lhv2 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ABC\"}");
            resultado = Titere.Perform("lhv3 = 12.34; Print lhv3 'X';");
            Assert.AreEqual(resultado, "{\"X\":12.34}");
            resultado = Titere.Perform("lhv4 = true; Print lhv4 'X';");
            Assert.AreEqual(resultado, "{\"X\":true}");
            resultado = Titere.Perform("C = Clase(); C.LHV = 'ZZZZZZ'; Print C.LHV 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ZZZZZZ\"}");
            resultado = Titere.Perform("C = Clase(); C.GetSetLHV = 'ZWYYWZ'; Print C.GetSetLHV 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ZWYYWZ\"}");
            resultado = Titere.Perform("C = Clase(); C.GetSetInternalLHv = 'ZWYYWZ'; Print C.getSetInternalLHV 'X';");
            Assert.AreEqual(resultado, "{\"X\":\"ZWYYWZ\"}");
            resultado = Titere.Perform("C = Clase(); C.Price = 1; Print C.Price 'X';");
            Assert.AreEqual(resultado, "{\"X\":1}");
            resultado = Titere.Perform("C = Clase(); C.Price = 1.0; Print C.Price 'X';");
            Assert.AreEqual(resultado, "{\"X\":1}");
            resultado = Titere.Perform("C = Clase(); C.Price = 1.23; Print C.Price 'X';");
            Assert.AreEqual(resultado, "{\"X\":1.23}");
            resultado = Titere.Perform("C = Clase(); C.Lvalue = 2; print C.LValue 'valor';");
            Assert.AreEqual(resultado, "{\"valor\":2}");
        }

        [TestMethod]
        public void ArgumentosVrsParametros()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase(); Print C.GetMetodoConDecimal('ABYZ', 1) 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"ABYZ1\"}}");
            resultado = Titere.Perform("C = Clase(); Print C.GetMetodoConDouble(3.21) 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"unValorDouble3.21\"}}");
            resultado = Titere.Perform("C = Clase(); Print C.GetMetodoConDecimal('ABYZ', 3.21) 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"ABYZ3,21\"}}");
            resultado = Titere.Perform("C = Clase(); Print C.GetMetodoConBoolean('ABYZ', true, 3.21) 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"ABYZTrue3,21\"}}");
        }

        [TestMethod]
        public void HerenciaEntreClaseDeTipoObjeto()
        {
            String resultado;
            resultado = Titere.Perform("C = ClaseHeredaDeBase();  Print C.FooCastHaciaClaseBase(C) 'X';");
            Assert.AreEqual(resultado, "{\"X\":1}");
            resultado = Titere.Perform("C = ClaseHeredaDeBase();  Print C.FooSinCastHaciaHeredaDeBase(C) 'X';");
            Assert.AreEqual(resultado, "{\"X\":{\"Clase\":\"ClaseHeredaDeBase\"}}");
        }

        [TestMethod]
        public void ComandoCall()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase();  C.Foo(); print C.PasePorFoo 'A';");
            Assert.AreEqual(resultado, "{\"A\":\"SI PASE\"}");
        }

        [TestMethod]
        public void ExpresionesDecimal()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase(); C.DecimalAmount = 0.1 + 0.9; Print C.DecimalAmount / 1.0 'X';");
            Assert.AreEqual(resultado, "{\"X\":1}");
            resultado = Titere.Perform("C = Clase(); C.DecimalAmount = 0.1 + 0.9 + 2; Print C.DecimalAmount / C.GetDecimalAmount(2) 'X';");
            Assert.AreEqual(resultado, "{\"X\":1.5}");
        }

        [TestMethod]
        public void Anuevo()
        {
            String resultado;
            resultado = Titere.Perform("C = Clase(); C.FechaHora = 31/03/2018 11:12:13; print C.FechaHora 'hora';");
            //Assert.AreEqual(resultado, "{\"hora\":\"12/11/2019 11:12:13\"}");
            //falla estilo: print a==b && ! c nombre; y tratando de encerrar con parentesis de eso
            resultado += "";
        }
    }
}
