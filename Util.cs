
namespace OpenTKMiniEngine; 

public static class Util {

	public static int FindElementIndex<T>(this IReadOnlyCollection<T> collection, Func<T, bool> evaluation) {
		
		for(int i = 0; i < collection.Count; i++) {
			if(evaluation(collection.ElementAt(i))) {
				return i;
			}
		}

		return -1;
	}
}
