using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

namespace CodeTools{

	public enum CodeSymbolType{
		segmentation,//分割
		assignment,//赋值
		operation,//运算
		block,//块
	}
	/// <summary>
	/// Code symbol.符号 '=' '[' ']' '{' ',' ';'
	/// </summary>
	public class CodeSymbol{

		CodeSymbolType type;
		public int index;
		public char symbol;

		public CodeSymbol(int index, char symbol){
			this.index = index;
			this.symbol = symbol;
		}
	}
	/// <summary>
	/// Code key word.关键字
	/// </summary>
	public class CodeKeyWord{

		public int index;
		public string keyWord;

		public CodeKeyWord(int index,string keyWord){
			this.index = index;
			this.keyWord = keyWord;
		}
	}
	/// <summary>
	/// Code element.代码段
	/// </summary>
	public class CodeFragment{
		public int charStartIndex;
		public int charEndIndex;
		public string codeFull;
		public string code;
	}
	/// <summary>
	/// Code file line.行
	/// </summary>
	public class CodeFileLine :CodeFragment{
		public int lineIndex;
	}
	/// <summary>
	/// Code block.块
	/// </summary>
	public class CodeBlock:CodeFragment{
		public string key;
		public CodeBlock parent;
		public List<CodeBlock> child = new List<CodeBlock>();
	}
	/// <summary>
	/// Code file.文件对象
	/// </summary>
	public class CodeFile {
		public string path;
		public string code;

		public CodeFile(string path,string code){
			this.path = path;
			this.code = code;
		}

		public HashSet<CodeSymbol> symbol = new HashSet<CodeSymbol> ();
		public HashSet<CodeKeyWord> keyWord = new HashSet<CodeKeyWord> ();

		public List<CodeFileLine> lines = new List<CodeFileLine>();

		public List<CodeBlock> child = new List<CodeBlock>();
	}

	public delegate int ResolverSymbolCallBack(CodeSymbol codeSymbol);
	public delegate int ResolverkeyWordCallBack(CodeKeyWord codeKeyWord);

	public class CodeResolver{

		public char[] symbolChar; 
		public string[] keyWordChar;

		Dictionary<char,ResolverSymbolCallBack> symbolCallBackSet = new Dictionary<char,ResolverSymbolCallBack>();
		Dictionary<string,ResolverkeyWordCallBack> keyWordCallBackSet = new Dictionary<string,ResolverkeyWordCallBack>();

		Stack<CodeSymbol> symbol = new Stack<CodeSymbol>();

		public CodeResolver(){
			
		}

		public CodeResolver( char[] symbolChar,string[] keyWordChar){
			this.symbolChar = symbolChar;
			this.keyWordChar = keyWordChar;
		}

		public void Init(){
			
			for(int i = 0;i < symbolChar.Length; i++){
				symbolCallBackSet [symbolChar [i]] = SymbolCallBack;
			}

			for(int i = 0;i < keyWordChar.Length; i++){
				keyWordCallBackSet [keyWordChar [i]] = keyWordCallBack;
			}
		}

		public void SetSymbolCallBack(char symbolChar,ResolverSymbolCallBack symbolCallBack){
			symbolCallBackSet [symbolChar] = symbolCallBack;
		}

		public void SetkeyWordCallBack(string keyWordStr,ResolverkeyWordCallBack resolverkeyWordCallBack){
			keyWordCallBackSet [keyWordStr] = resolverkeyWordCallBack;
		}

		public void Execute(CodeFile codeFile){

			char[] charArr = codeFile.code.ToCharArray();

			CodeSymbol curCodeSymbol;
			int curIndex = 0;

			while (curIndex < charArr.Length) {
				
				curCodeSymbol = CodeFileUtils.FindNextSymbol (charArr,curIndex,symbolCallBackSet);
				if (curCodeSymbol == null)
					break;

				curIndex = symbolCallBackSet [curCodeSymbol.symbol] (curCodeSymbol);
			}
		}

		protected virtual int SymbolCallBack(CodeSymbol codeSymbol){

			return ++codeSymbol.index;
		}

		protected virtual int keyWordCallBack(CodeKeyWord codeKeyWord){

			return ++codeKeyWord.index;
		}
	}

	public class CodeFileUtils {

		public static CodeBlock FindBlock(string code,int startIndex,string startFlag,string endFlag){
			
			int blockStartIndex = code.IndexOf (startFlag, startIndex);
			if (blockStartIndex == -1)
				return null;
			int blockEndIndex = code.IndexOf (endFlag, blockStartIndex);
			if (blockEndIndex == -1)
				return null;

			CodeBlock codeBlock = new CodeBlock ();
			codeBlock.charStartIndex = blockStartIndex;
			codeBlock.charEndIndex = blockEndIndex;
			codeBlock.codeFull = code;
			codeBlock.code = code.Substring (blockStartIndex, blockEndIndex - blockStartIndex);

			return codeBlock;
		}

		static public CodeSymbol FindNextSymbol(char[] charArr,int startIndex,Dictionary<char,ResolverSymbolCallBack> symbolCallBackSet){

			int curIndex = startIndex;

			while (curIndex < charArr.Length) {

				char c = charArr [curIndex];

				if(symbolCallBackSet.ContainsKey(c)){
					return new CodeSymbol(curIndex,c);
				}

				curIndex ++;
			}

			return null;
		}

		static public void Resolving(string path){

			string code = File.ReadAllText (path);
			CodeFile codeFile = new CodeFile (path, code);

			char[] symbolChar = new char[] {'=','>','<','+','-','*','/', 
											'[',']','{','}','(',')',
											';',' ','\r','\n','\t','/','-'}; 
			string[] keyWordChar = new string[] {"local","function","end","if","else","elseif","then"};

			CodeResolver codeResolver = new CodeResolver (symbolChar,keyWordChar);
			codeResolver.Init ();
			
		}
	}
}