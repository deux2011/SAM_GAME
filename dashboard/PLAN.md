# Code Mate 서비스 대시보드 구현 계획

## 1. 개요

DS부문의 AI Coding 도구 "Code Mate" 전반에 대한 서비스 사용 현황을 모니터링하는 대시보드를 구축합니다.

---

## 2. 요구사항 정리

### 2.1 사용자 정보 (User Profile)
| 항목 | 설명 |
|------|------|
| 소속 | 사용자가 속한 조직/팀 |
| 직군 | S직군 등 직군 분류 |
| 구현 개발자 여부 | Citizen 개발자 vs 구현 개발자 구분 |

### 2.2 사용자 모수 (User Population)
| 항목 | 설명 |
|------|------|
| DS부문 S직군 인원 수 | S직군 전체 인원 |
| DS부문 Citizen 개발자 인원 수 | Citizen 개발자 인원 |
| DS부문 인원 수 | DS부문 전체 인원 |

### 2.3 서비스 분류
| 서비스 | 하위 도구 |
|--------|-----------|
| **Code Mate** | Continue, Roo Code, OpenCode |
| **Code Pearl** | - |
| **Code Search** | - |

### 2.4 지원 모델
- GLM 4.7
- Qwen 3.5

### 2.5 요청 관련 지표
| 지표 | 설명 |
|------|------|
| 요청 수 | 서비스별/모델별 총 요청 건수 |
| 요청별 응답 시간 | 평균/P50/P95/P99 응답 시간 |
| 요청별 Token | Input Token, Output Token, Cache Hit Token |

### 2.6 코드 관련 지표
| 지표 | 설명 |
|------|------|
| 코드 적용 라인 수 | AI가 생성한 코드 중 실제 적용된 라인 수 |

### 2.7 시스템 관련 지표
| 지표 | 설명 |
|------|------|
| GPU 전체 개수 | 클러스터 내 총 GPU 수 |
| GPU당 모델 탑재 현황 | 각 GPU에 배포된 모델 정보 |
| 모델과 서비스 연결 현황 | 어떤 모델이 어떤 서비스에 연결되어 있는지 |

---

## 3. 기술 스택

| 구분 | 선택 | 사유 |
|------|------|------|
| **Framework** | React 18 + TypeScript | 컴포넌트 기반, 타입 안전성 |
| **빌드 도구** | Vite | 빠른 HMR, 간단한 설정 |
| **UI 라이브러리** | Ant Design 5 | 대시보드에 최적화된 테이블/카드/레이아웃 |
| **차트 라이브러리** | Apache ECharts (echarts-for-react) | 풍부한 차트 유형, 한글 지원 |
| **상태관리** | Zustand | 경량, 간결한 API |
| **API 통신** | Axios + React Query | 캐싱, 자동 재요청, 로딩 상태 관리 |
| **라우팅** | React Router v6 | SPA 라우팅 |
| **스타일링** | CSS Modules + Ant Design Token | 일관된 디자인 시스템 |

---

## 4. 디렉토리 구조

```
dashboard/
├── public/
├── src/
│   ├── api/                    # API 인터페이스
│   │   ├── client.ts           # Axios 인스턴스
│   │   ├── userApi.ts          # 사용자 관련 API
│   │   ├── metricsApi.ts       # 요청/코드 지표 API
│   │   └── systemApi.ts        # 시스템 지표 API
│   │
│   ├── components/             # 공통 컴포넌트
│   │   ├── Layout/
│   │   │   ├── AppLayout.tsx   # 전체 레이아웃 (Sider + Header + Content)
│   │   │   └── Sidebar.tsx     # 사이드바 네비게이션
│   │   ├── Charts/
│   │   │   ├── RequestChart.tsx       # 요청 수 추이 차트
│   │   │   ├── ResponseTimeChart.tsx  # 응답 시간 차트
│   │   │   ├── TokenUsageChart.tsx    # 토큰 사용량 차트
│   │   │   ├── CodeLineChart.tsx      # 코드 적용 라인 차트
│   │   │   └── GpuStatusChart.tsx     # GPU 현황 차트
│   │   ├── Cards/
│   │   │   ├── StatCard.tsx           # 통계 카드 (KPI)
│   │   │   └── ServiceCard.tsx        # 서비스별 요약 카드
│   │   └── Tables/
│   │       ├── UserTable.tsx          # 사용자 목록 테이블
│   │       ├── GpuTable.tsx           # GPU 모델 탑재 현황 테이블
│   │       └── ServiceModelTable.tsx  # 서비스-모델 연결 테이블
│   │
│   ├── pages/                  # 페이지 컴포넌트
│   │   ├── Overview/
│   │   │   └── OverviewPage.tsx       # 전체 요약 대시보드
│   │   ├── ServiceMetrics/
│   │   │   └── ServiceMetricsPage.tsx # 서비스별 상세 지표
│   │   ├── UserAnalytics/
│   │   │   └── UserAnalyticsPage.tsx  # 사용자 분석
│   │   └── SystemStatus/
│   │       └── SystemStatusPage.tsx   # 시스템 현황
│   │
│   ├── stores/                 # Zustand 상태 관리
│   │   ├── filterStore.ts      # 필터 상태 (기간, 서비스, 모델)
│   │   └── userStore.ts        # 사용자 정보 상태
│   │
│   ├── types/                  # TypeScript 타입 정의
│   │   ├── user.ts
│   │   ├── metrics.ts
│   │   └── system.ts
│   │
│   ├── utils/                  # 유틸리티
│   │   └── formatters.ts       # 숫자/날짜 포맷터
│   │
│   ├── App.tsx
│   ├── main.tsx
│   └── router.tsx
│
├── package.json
├── tsconfig.json
├── vite.config.ts
└── index.html
```

---

## 5. 페이지 구성

### 5.1 Overview (전체 요약 대시보드)
메인 화면. 한눈에 전체 서비스 현황을 파악할 수 있는 페이지.

```
┌─────────────────────────────────────────────────────────────┐
│  [필터 바] 기간 선택 | 서비스 선택 | 모델 선택              │
├──────────┬──────────┬──────────┬──────────┬─────────────────┤
│ KPI 카드 │ KPI 카드 │ KPI 카드 │ KPI 카드 │ KPI 카드        │
│ 총 요청수│ 평균응답 │ 총 Token │ 코드적용 │ 활성사용자      │
│          │ 시간     │ 사용량   │ 라인수   │                 │
├──────────┴──────────┴──────────┴──────────┴─────────────────┤
│  [요청 수 추이 차트]              │  [서비스별 요청 비율]    │
│  Line Chart (일별/주별/월별)      │  Pie/Donut Chart        │
├───────────────────────────────────┼─────────────────────────┤
│  [모델별 Token 사용량]            │  [응답 시간 분포]       │
│  Stacked Bar Chart                │  Box Plot / Histogram   │
├───────────────────────────────────┴─────────────────────────┤
│  [사용자 모수 현황]                                         │
│  DS부문 전체 | S직군 | Citizen 개발자 (Progress Bar)        │
└─────────────────────────────────────────────────────────────┘
```

**KPI 카드 (5개)**:
- 총 요청 수 (전일 대비 증감률)
- 평균 응답 시간 (ms)
- 총 Token 사용량 (Input + Output)
- 코드 적용 라인 수
- 활성 사용자 수

### 5.2 Service Metrics (서비스별 상세 지표)
서비스별로 드릴다운하여 상세 지표를 확인하는 페이지.

```
┌─────────────────────────────────────────────────────────────┐
│  [탭] Code Mate | Code Pearl | Code Search                  │
├─────────────────────────────────────────────────────────────┤
│  [Code Mate 하위 탭] Continue | Roo Code | OpenCode | 전체  │
├──────────┬──────────┬──────────┬────────────────────────────┤
│ 요청 수  │ 응답시간 │ Token    │ 코드 적용 라인             │
│ (일별)   │ P50/P95  │ I/O/캐시 │ (일별 추이)               │
├──────────┴──────────┴──────────┴────────────────────────────┤
│  [요청 추이 상세]                                           │
│  Multi-line Chart (모델별 비교)                              │
├─────────────────────────────────────────────────────────────┤
│  [Token 상세 분석]                                          │
│  Stacked Area Chart: Input / Output / Cache Hit             │
├─────────────────────────────────────────────────────────────┤
│  [응답 시간 백분위]                                         │
│  P50 / P95 / P99 트렌드 라인                                │
└─────────────────────────────────────────────────────────────┘
```

### 5.3 User Analytics (사용자 분석)
사용자 분포와 활용 현황을 분석하는 페이지.

```
┌─────────────────────────────────────────────────────────────┐
│  [사용자 모수 요약]                                         │
│  DS부문 전체: N명 | S직군: N명 | Citizen 개발자: N명        │
├─────────────────────────────────┬───────────────────────────┤
│  [직군별 사용 비율]             │  [소속별 사용 현황]       │
│  Bar Chart                      │  Horizontal Bar Chart     │
├─────────────────────────────────┼───────────────────────────┤
│  [구현 개발자 vs Citizen 개발자]│  [서비스별 사용자 분포]   │
│  비교 차트                      │  Grouped Bar Chart        │
├─────────────────────────────────┴───────────────────────────┤
│  [사용자 상세 테이블]                                       │
│  소속 | 직군 | 구현개발자여부 | 사용서비스 | 요청수 | ...   │
└─────────────────────────────────────────────────────────────┘
```

### 5.4 System Status (시스템 현황)
GPU 및 모델 배포/연결 현황을 모니터링하는 페이지.

```
┌─────────────────────────────────────────────────────────────┐
│  [GPU 요약]  전체 GPU: N개 | 활성: N개 | 유휴: N개          │
├─────────────────────────────────────────────────────────────┤
│  [GPU당 모델 탑재 현황]                                     │
│  ┌─────────┬────────────┬────────┬───────┐                  │
│  │ GPU ID  │ 모델       │ 상태   │ 사용률│                  │
│  │ GPU-001 │ GLM 4.7    │ Active │ 78%   │                  │
│  │ GPU-002 │ Qwen 3.5   │ Active │ 65%   │                  │
│  │ ...     │ ...        │ ...    │ ...   │                  │
│  └─────────┴────────────┴────────┴───────┘                  │
├─────────────────────────────────────────────────────────────┤
│  [모델 ↔ 서비스 연결 현황]                                  │
│                                                             │
│  ┌──────────┐     ┌───────────┐     ┌──────────────┐       │
│  │ GLM 4.7  │────▶│ Code Mate │────▶│ Continue     │       │
│  │          │────▶│           │────▶│ Roo Code     │       │
│  │          │────▶│ Code Pearl│     │ OpenCode     │       │
│  ├──────────┤     ├───────────┤     └──────────────┘       │
│  │ Qwen 3.5 │────▶│ Code Mate │                            │
│  │          │────▶│ CodeSearch│                             │
│  └──────────┘     └───────────┘                             │
│  (Sankey Diagram 또는 연결 테이블)                           │
└─────────────────────────────────────────────────────────────┘
```

---

## 6. 데이터 모델 (TypeScript 타입)

### 6.1 사용자 관련

```typescript
// types/user.ts
interface User {
  id: string;
  name: string;
  department: string;       // 소속
  jobGroup: string;         // 직군 (예: 'S직군')
  isDeveloper: boolean;     // 구현 개발자 여부 (false = Citizen 개발자)
}

interface UserPopulation {
  totalDSDepartment: number;       // DS부문 전체 인원
  sJobGroupCount: number;          // DS부문 S직군 인원
  citizenDeveloperCount: number;   // DS부문 Citizen 개발자 인원
}
```

### 6.2 요청 지표 관련

```typescript
// types/metrics.ts
type ServiceType = 'code-mate' | 'code-pearl' | 'code-search';
type CodeMateToolType = 'continue' | 'roo-code' | 'open-code';
type ModelType = 'glm-4.7' | 'qwen-3.5';

interface RequestMetrics {
  service: ServiceType;
  tool?: CodeMateToolType;    // Code Mate 하위 도구
  model: ModelType;
  date: string;               // ISO date
  requestCount: number;
  responseTime: {
    avg: number;
    p50: number;
    p95: number;
    p99: number;
  };
  tokens: {
    input: number;
    output: number;
    cacheHit: number;
  };
}

interface CodeMetrics {
  service: ServiceType;
  tool?: CodeMateToolType;
  date: string;
  appliedLines: number;       // 코드 적용 라인 수
}
```

### 6.3 시스템 관련

```typescript
// types/system.ts
interface GpuInfo {
  gpuId: string;
  model: ModelType;
  status: 'active' | 'idle' | 'error';
  utilization: number;        // 0-100%
}

interface ModelServiceMapping {
  model: ModelType;
  services: {
    service: ServiceType;
    tools?: CodeMateToolType[];
  }[];
}

interface SystemOverview {
  totalGpuCount: number;
  activeGpuCount: number;
  gpuList: GpuInfo[];
  modelServiceMappings: ModelServiceMapping[];
}
```

---

## 7. API 엔드포인트 (예상)

| Method | Endpoint | 설명 |
|--------|----------|------|
| GET | `/api/users/population` | 사용자 모수 조회 |
| GET | `/api/users?dept=&jobGroup=&isDev=` | 사용자 목록 (필터) |
| GET | `/api/metrics/requests?service=&model=&from=&to=` | 요청 지표 조회 |
| GET | `/api/metrics/tokens?service=&model=&from=&to=` | 토큰 사용량 조회 |
| GET | `/api/metrics/response-time?service=&model=&from=&to=` | 응답 시간 조회 |
| GET | `/api/metrics/code-lines?service=&from=&to=` | 코드 적용 라인 조회 |
| GET | `/api/system/gpus` | GPU 현황 조회 |
| GET | `/api/system/model-service-mapping` | 모델-서비스 연결 현황 |
| GET | `/api/metrics/overview` | 대시보드 요약 KPI |

---

## 8. 구현 단계

### Phase 1: 프로젝트 초기화 및 레이아웃
1. Vite + React + TypeScript 프로젝트 생성
2. 의존성 설치 (Ant Design, ECharts, Zustand, React Query, Axios, React Router)
3. 디렉토리 구조 생성
4. AppLayout 컴포넌트 (Sider + Header + Content)
5. 라우터 설정 (4개 페이지)
6. Mock 데이터 준비

### Phase 2: 공통 컴포넌트
1. StatCard (KPI 카드) 컴포넌트
2. 필터 바 컴포넌트 (기간, 서비스, 모델 선택)
3. API 클라이언트 + React Query 훅 설정

### Phase 3: Overview 페이지
1. KPI 카드 5개 배치
2. 요청 수 추이 Line Chart
3. 서비스별 요청 비율 Pie Chart
4. 모델별 Token 사용량 Stacked Bar Chart
5. 응답 시간 분포 차트
6. 사용자 모수 현황 Progress Bar

### Phase 4: Service Metrics 페이지
1. 서비스/도구 탭 구성
2. 요청 추이 상세 차트 (모델별 비교)
3. Token 상세 분석 (Input/Output/Cache Hit)
4. 응답 시간 백분위 트렌드

### Phase 5: User Analytics 페이지
1. 사용자 모수 요약 카드
2. 직군별/소속별 사용 비율 차트
3. 개발자 유형별 비교 차트
4. 사용자 상세 테이블

### Phase 6: System Status 페이지
1. GPU 요약 카드
2. GPU당 모델 탑재 현황 테이블
3. 모델-서비스 연결 현황 (Sankey Diagram 또는 테이블)

---

## 9. 글로벌 필터

모든 페이지에서 사용 가능한 공통 필터:

| 필터 | 옵션 |
|------|------|
| **기간** | 오늘, 최근 7일, 최근 30일, 커스텀 범위 |
| **서비스** | 전체, Code Mate, Code Pearl, Code Search |
| **Code Mate 도구** | 전체, Continue, Roo Code, OpenCode |
| **모델** | 전체, GLM 4.7, Qwen 3.5 |
| **소속** | 전체, 부서 선택 |
| **직군** | 전체, S직군 등 |

---

## 10. Mock 데이터 전략

실제 API가 준비되기 전까지 Mock 데이터를 사용합니다:
- `src/mocks/` 디렉토리에 JSON 형태 mock 데이터 배치
- API 클라이언트에서 환경 변수로 mock/real 전환 가능
- 현실적인 데이터 범위로 생성하여 차트 검증 용이

---

## 11. 주요 고려사항

1. **반응형 레이아웃**: Ant Design Grid 시스템 활용, 태블릿/데스크톱 대응
2. **다크 모드**: Ant Design 테마 토큰으로 라이트/다크 전환 지원
3. **실시간 갱신**: React Query의 refetchInterval로 주기적 데이터 갱신 (30초~1분)
4. **데이터 내보내기**: 테이블 데이터 CSV 다운로드 기능
5. **로딩/에러 상태**: Skeleton 로딩 + Error Boundary 처리
