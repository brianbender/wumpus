import static org.junit.Assert.*;

import org.junit.Before;
import org.junit.Test;


public class MapTest {

	private Map testObj;

	@Before
	public void setUp() {
		testObj = new Map();
	}

	@Test
	public void testValidMoves() {
		assertTrue(testObj.isValidMove(5, 1));
		assertTrue(testObj.isValidMove(5, 4));
		assertTrue(testObj.isValidMove(5, 6));
	}
	
	@Test
	public void testInvalidMoves() throws Exception {
		int[] validMoves = {1,2,3,5,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
		for (int badMove : validMoves) {
			assertFalse(testObj.isValidMove(5, badMove));
		}
	}

}
