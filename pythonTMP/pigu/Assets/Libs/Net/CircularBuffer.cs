using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct DataFragment {
	public int startIndex;
	public int lenght;
}

public class CircularBuffer {

	public int position ;//{ get; private set; }
	public int len;

	private byte[] _byteArray;
	// 			  |1|2|3|4|5|6|7
	// N|N|N|N|N|N|N|N|N
	//             ^
	//			position
	//            |1|2|3
	//             ^
	//          position2
	// 4|5|6|7|N|N|1|2|3 
	// 		   ^
	//         position
	// 		   ^ ^
	//         len
	int position2;
	//int len2;

	private DataFragment[] _fragmentArray;
	private int curfragmentPosition = 0;
	private int curfragmentStart = -1;

	public CircularBuffer(int capacity){
		_byteArray = new byte[capacity];
		_fragmentArray = new DataFragment[capacity];
		position = 0;
		len = capacity;
	}

	public int Wirte(byte[] src,int srcOffSet,int srcLen){

		if (len < srcLen) {
			//缓冲区耗尽
			return -1;
		}

		if (position + srcLen < _byteArray.Length) {
			Array.Copy (src, 0,  _byteArray, position,srcLen);

			SetFragment (position,srcLen);

			position += srcLen;

		} else {
			Array.Copy (src, 0,  _byteArray, position, _byteArray.Length - position);
			Array.Copy (src, _byteArray.Length - position, _byteArray, 0,srcLen -( _byteArray.Length - position));

			SetFragment (position,srcLen);

			position = srcLen - (_byteArray.Length - position);

			len = position - position2;
		}

		return position;
	}

	private void SetFragment(int startIndex,int lenght){
		
		if (curfragmentStart < 0)
			curfragmentStart = 0;
			
		_fragmentArray [curfragmentPosition].startIndex = startIndex;
		_fragmentArray [curfragmentPosition].lenght = lenght;

		curfragmentPosition++;

		if (curfragmentPosition >= _fragmentArray.Length) {
			curfragmentPosition = 0;
		}
	}

	public bool DataFragmentHas(){
		return (curfragmentStart < curfragmentPosition);
	}

	public int DataFragmentLen(){
		return curfragmentPosition - curfragmentStart;
	}

	public DataFragment DataFragmentNext(){
		return _fragmentArray [curfragmentStart++]; 
	}
}
