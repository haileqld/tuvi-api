import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

const resources = {
  en: {
    translation: {
      app: {
        title: 'Tuvi Horoscope',
        onboarding: 'Welcome to Tuvi Horoscope. Enter your birth details to receive a detailed Vietnamese horoscope interpretation based on the Nam Phái tradition.',
      },
      form: {
        name: 'Name (Optional)',
        gender: 'Gender',
        male: 'Male',
        female: 'Female',
        birthDate: 'Birth Date and Time',
        timezone: 'Timezone Offset (hours)',
        includeTechnical: 'Include Technical Details',
        submit: 'Generate Horoscope',
        validating: 'Validating...',
        generating: 'Generating Horoscope...',
      },
      result: {
        interpretation: 'Interpretation',
        technicalChart: 'Technical Chart',
        palace: 'Palace',
        location: 'Location',
        stars: 'Stars',
        error: 'An error occurred while generating the horoscope.',
      },
      common: {
        error: 'Error',
        retry: 'Retry',
      }
    },
  },
  vi: {
    translation: {
      app: {
        title: 'Tử Vi Số Mệnh',
        onboarding: 'Chào mừng bạn đến với Tử Vi Số Mệnh. Nhập thông tin ngày sinh để nhận bản luận giải tử vi chi tiết theo truyền thống Nam Phái.',
      },
      form: {
        name: 'Họ tên (Tùy chọn)',
        gender: 'Giới tính',
        male: 'Nam',
        female: 'Nữ',
        birthDate: 'Ngày giờ sinh',
        timezone: 'Múi giờ (giờ)',
        includeTechnical: 'Bao gồm chi tiết kỹ thuật',
        submit: 'Lấy lá số',
        validating: 'Đang kiểm tra...',
        generating: 'Đang luận giải...',
      },
      result: {
        interpretation: 'Luận giải',
        technicalChart: 'Lá số chi tiết',
        palace: 'Cung',
        location: 'Vị trí',
        stars: 'Sao',
        error: 'Đã xảy ra lỗi khi lấy lá số.',
      },
      common: {
        error: 'Lỗi',
        retry: 'Thử lại',
      }
    },
  },
};

i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources,
    fallbackLng: 'en',
    interpolation: {
      escapeValue: false,
    },
  });

export default i18n;
