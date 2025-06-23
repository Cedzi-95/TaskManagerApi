/**
 * Formats a date into "year-month-day hour:minute" format
 * @param {Date} date - The date to format
 * @returns {string} Formatted date string
 */
export function formatDatePretty(date) {
    // Get components
    const year = date.getFullYear();
  
    // Month is 0-indexed, so add 1 and pad with leading zero if needed
    const month = String(date.getMonth() + 1).padStart(2, "0");
  
    // Get day and pad with leading zero if needed
    const day = String(date.getDate()).padStart(2, "0");
  
    // Get hours and pad with leading zero if needed
    const hours = String(date.getHours()).padStart(2, "0");
  
    // Get minutes and pad with leading zero if needed
    const minutes = String(date.getMinutes()).padStart(2, "0");
  
    // Return formatted string
    return `${year}-${month}-${day} ${hours}:${minutes}`;
  }